using AutoMapper;
using LinqKit;
using Microsoft.Extensions.Logging;
using ServiceQuotes.Application.DTOs.Payment;
using ServiceQuotes.Application.DTOs.Paynow;
using ServiceQuotes.Application.Exceptions;
using ServiceQuotes.Application.Filters;
using ServiceQuotes.Application.Interfaces;
using ServiceQuotes.Domain;
using ServiceQuotes.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Net.Http;
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

        public PaymentService(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            ILogger<PaymentService> logger,
            IHttpClientFactory clientFactory)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _clientFactory = clientFactory;
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

        public async Task<PaynowPaymentResponse> CreatePaynowPayment(CreatePaynowPaymentRequest dto)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "repos/dotnet/AspNetCore.Docs/pulls");

            var client = _clientFactory.CreateClient("paynow");

            var response = await client.SendAsync(request);
            using var responseStream = await response.Content.ReadAsStreamAsync();

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await JsonSerializer.DeserializeAsync<PaynowErrorResponse>(responseStream);
                _logger.LogError("Create PayNow payment error", errorResponse);
                throw new AppException("Creating payment was not successfull.");
            }

            return await JsonSerializer.DeserializeAsync<PaynowPaymentResponse>(responseStream);
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
    }
}
