using AutoMapper;
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
using System.Linq;
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
        private readonly IQuoteService _quoteService;

        public PaymentService(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            ILogger<PaymentService> logger,
            IHttpClientFactory clientFactory,
            IOptions<AppSettings> appSettings,
            IQuoteService quoteService)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _clientFactory = clientFactory;
            _appSettings = appSettings.Value;
            _quoteService = quoteService;
        }

        public async Task<List<GetPaymentResponse>> GetAllPayments(Account account, GetPaymentsFilter filter)
        {
            // prepare filter predicate
            var predicate = PredicateBuilder.New<Payment>(true);

            if (!string.IsNullOrEmpty(filter?.SearchString))
            {
                // Special case for Quote Ref Number. Quote Ref Number is displayed with # to the end user.
                if (filter.SearchString.StartsWith("#"))
                {
                    // If searchString starts with # it gives a sign that user wants to search by Quote Ref Number.
                    // We need to remove # since it is not stored in DB.
                    predicate = predicate.Or(p => p.Quote.ReferenceNumber.ToString().ToLower().Contains(filter.SearchString.Remove(0,1).ToLower()));
                } else
                {
                    predicate = predicate.Or(p => p.Provider.ToLower().Contains(filter.SearchString.ToLower()));
                    predicate = predicate.Or(p => p.TransactionId.ToLower().Contains(filter.SearchString.ToLower()));
                    predicate = predicate.Or(p => p.Amount.ToString().ToLower().Contains(filter.SearchString.ToLower()));
                    predicate = predicate.Or(p => p.Customer.CompanyName.ToLower().Contains(filter.SearchString.ToLower()));
                }
            }

            if (account.Role == Role.Customer)
            {
                // customer can get only his own payments
                var customer = await _unitOfWork.Customers.GetByAccountId(account.Id);
                predicate = predicate.And(p => p.CustomerId == customer.Id);
            }

            if (!string.IsNullOrEmpty(filter?.DateRange))
            {
                switch (filter.DateRange)
                {
                    case "30-days":
                        predicate = predicate.And(p => p.Created >= DateTime.UtcNow.AddDays(-30));
                        break;
                    case "7-days":
                        predicate = predicate.And(p => p.Created >= DateTime.UtcNow.AddDays(-7));
                        break;
                    case "today":
                        predicate = predicate.And(p => p.Created.Date == DateTime.UtcNow.Date);
                        break;
                }
            }

            if (filter.Status is not null)
            {
                predicate = predicate.And(p => p.Status.Equals(filter.Status));
            }

            var payment = await _unitOfWork.Payments.FindWithQuoteAndCustomer(predicate);

            return _mapper.Map<List<GetPaymentResponse>>(payment);
        }

        public async Task<GetPaymentResponse> GetPaymentById(Guid id)
        {
            var payment = await _unitOfWork.Payments.GetWithQuoteAndCustomer(id);

            if (payment is null)
                throw new KeyNotFoundException("Payment not found");

            var paymentResponse = _mapper.Map<GetPaymentResponse>(payment);
            paymentResponse.Customer.Image = payment.Customer.Account.Image;
            return paymentResponse;
        }

        public async Task<GetPaymentResponse> GetPaymentByTransactionId(string id)
        {
            var payment = await _unitOfWork.Payments.GetWithQuoteAndCustomer(id);

            if (payment is null)
                throw new KeyNotFoundException("Payment not found");

            var paymentResponse = _mapper.Map<GetPaymentResponse>(payment);
            paymentResponse.Customer.Image = payment.Customer.Account.Image;
            return paymentResponse;
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

        public async Task<bool> ProcessPaynowNotification(PaynowNotificationRequest dto, string signature)
        {
            // validate signature header
            string serializedData = JsonSerializer.Serialize(dto);
            string calculatedSignature = CalculatePaynowSignature(serializedData);

            if (!String.Equals(signature, calculatedSignature))
                throw new AppException("Calculated signature is not matching header signature");

            var payment = await _unitOfWork.Payments.Get(Guid.Parse(dto.ExternalId));

            if (payment is null)
                throw new KeyNotFoundException("Payment not found.");

            // convert string to Status enum type
            Status parsedStatus = (Status)Enum.Parse(typeof(Status), dto.Status, true);

            // update payment status
            if (ShouldStatusBeUpdated(payment.Status, parsedStatus))
            {
                payment.Status = parsedStatus;
                payment.Updated = DateTime.UtcNow;

                // if payment is confirmed update quote status
                if (payment.Status == Status.Confirmed)
                {
                    await _quoteService.UpdateQuoteStatus(payment.QuoteId, Status.Paid);
                }

                _unitOfWork.Commit();

                return true;
            }

            return false;
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
            var signature = CalculatePaynowSignature(data);

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

        private string CalculatePaynowSignature(string data)
        {
            var encoder = new System.Text.UTF8Encoding();
            return Utilities.CalculateHMAC(encoder.GetBytes(_appSettings.PaynowApiSignatureKey), encoder.GetBytes(data));
        }

        // paynow status notifications can come out of order
        // so this method determines if payment status should be updated
        // for example status New shouldn't be applied after Pending or Completed
        private bool ShouldStatusBeUpdated(Status currentStatus, Status newStatus)
        {
            // array of available statuses filled in correct order
            Status[] correctStatuses = new Status[6] {
                Status.New, Status.Pending, Status.Rejected, Status.Expired, Status.Error, Status.Confirmed
            };

            // check if newStatus has correct value
            if (!correctStatuses.Contains(newStatus))
                return false;

            // get current and new status index in array
            int currentIndex = Array.IndexOf(correctStatuses, currentStatus);
            int newIndex = Array.IndexOf(correctStatuses, newStatus);

            if (newIndex <= currentIndex)
                return false;

            return true;
        }
    }
}
