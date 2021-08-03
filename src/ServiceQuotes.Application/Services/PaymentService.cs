﻿using AutoMapper;
using LinqKit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServiceQuotes.Application.DTOs.Payment;
using ServiceQuotes.Application.DTOs.Paynow;
using ServiceQuotes.Application.Exceptions;
using ServiceQuotes.Application.Filters;
using ServiceQuotes.Application.Helpers;
using ServiceQuotes.Application.Interfaces;
using ServiceQuotes.Domain;
using ServiceQuotes.Domain.Entities;
using ServiceQuotes.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ServiceQuotes.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpClientFactory _clientFactory;
        private readonly AppSettings _appSettings;

        public PaymentService(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            ILogger<PaymentService> logger,
            IHttpClientFactory clientFactory,
            IOptions<AppSettings> appSettings)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _clientFactory = clientFactory;
            _appSettings = appSettings.Value;
        }

        public async Task<List<GetPaymentResponse>> GetAllPayments(GetPaymentsFilter filter)
        {
            // prepare filter predicate
            var predicate = PredicateBuilder.New<Payment>(true);

            if (!string.IsNullOrEmpty(filter?.SearchString))
            {
                predicate = predicate.Or(p => p.Provider.Equals(filter.SearchString.ToLower()));
                predicate = predicate.Or(p => p.TransactionId.Equals(filter.SearchString.ToLower()));
                predicate = predicate.Or(p => p.Status.Equals(filter.SearchString.ToLower()));
            }

            var payment = await _unitOfWork.Payments.Find(predicate);

            return _mapper.Map<List<GetPaymentResponse>>(payment);
        }

        public async Task<GetPaymentResponse> GetPaymentById(Guid id)
        {
            return _mapper.Map<GetPaymentResponse>(await _unitOfWork.Payments.Get(id));
        }

        public async Task<List<GetPaymentResponse>> GetPaymentsByCustomerId(Guid id)
        {
            return _mapper.Map<List<GetPaymentResponse>>(await _unitOfWork.Payments.Find(p => p.CustomerId.Equals(id)));
        }

        public async Task<List<GetPaymentResponse>> GetPaymentsByQuoteId(Guid id)
        {
            return _mapper.Map<List<GetPaymentResponse>>(await _unitOfWork.Payments.Find(p => p.QuoteId.Equals(id)));
        }

        public async Task<GetPaymentResponse> GetPaymentForQuote(Guid quoteId)
        {
            var payment = await _unitOfWork.Payments.SingleOrDefault(p => p.QuoteId == quoteId);
            return _mapper.Map<GetPaymentResponse>(payment);
        }

        public async Task<CreatePaymentResponse> CreatePaymentForQuote(CreatePaymentRequest dto, Guid accountId)
        {
            var customer = await _unitOfWork.Customers.GetByAccountId(accountId);
            var quote = await _unitOfWork.Quotes.Get(Guid.Parse(dto.QuoteId));

            // validate
            if (quote is null)
                throw new KeyNotFoundException("Quote does not exist.");

            if (quote.Status == Status.Paid)
                throw new AppException("This quote has already been paid.");

            if (dto.Provider != "Paynow")
                throw new AppException("Currently only Paynow provider is supported.");

            var service = await _unitOfWork.ServiceRequests.Get(quote.ServiceRequestId);

            if (service.CustomerId != customer.Id)
                throw new AppException("You are not allowed to pay for this quote.");

            var internalPayment = await CreatePayment(quote, service.CustomerId, "Paynow");
            var paynowPayment = await CreatePaynowPayment(new CreatePaynowPaymentRequest
            {
                // amount must be passed in smallest currency unit (grosz in the case of PLN)
                Amount = Decimal.ToInt32(internalPayment.Amount * 100),
                Currency = Currency.PLN.ToString(),
                Description = $"Payment for quote #{quote.ReferenceNumber}.",
                ExternalId = internalPayment.Id,
                Buyer = new Buyer
                {
                    Email = customer.Account.Email
                }
            });

            internalPayment.TransactionId = paynowPayment.PaymentId;
            _unitOfWork.Commit();

            return new CreatePaymentResponse
            {
                RedirectUrl = paynowPayment.RedirectUrl,
                Payment = await GetPaymentById(internalPayment.Id)
            };
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _unitOfWork.Dispose();
            }
        }

        private async Task<Payment> CreatePayment(Quote quote, Guid customerId, string provider)
        {
            Guid newPaymentId = Guid.NewGuid();

            var newPayment = new Payment
            {
                Id = newPaymentId,
                Amount = quote.Total,
                Status = Status.New,
                QuoteId = quote.Id,
                CustomerId = customerId,
                Provider = provider,
                Created = DateTime.UtcNow
            };

            _unitOfWork.Payments.Add(newPayment);
            _unitOfWork.Commit();

            return await _unitOfWork.Payments.Get(newPaymentId);
        }

        private async Task<PaynowPaymentResponse> CreatePaynowPayment(CreatePaynowPaymentRequest dto)
        {
            var client = _clientFactory.CreateClient("paynow");

            var data = JsonSerializer.Serialize(dto);
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            var encoder = new System.Text.UTF8Encoding();
            var signature = Utilities.CalculateHMAC(encoder.GetBytes(_appSettings.PaynowApiSignatureKey), encoder.GetBytes(data));

            client.DefaultRequestHeaders.Add("Signature", signature);
            client.DefaultRequestHeaders.Add("Idempotency-Key", dto.ExternalId.ToString());

            var response = await client.PostAsync("/v1/payments", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<PaynowErrorResponse>();
                _logger.LogError($"Create PayNow payment error response {@errorResponse}", errorResponse);
                throw new AppException("Creating payment was not successfull.");
            }

            return await response.Content.ReadFromJsonAsync<PaynowPaymentResponse>();
        }
    }
}
