using ServiceQuotes.Application.DTOs.Customer;
using ServiceQuotes.Application.DTOs.Quote;
using System;

namespace ServiceQuotes.Application.DTOs.Payment
{
    public class GetPaymentResponse
    {
        public Guid Id { get; set; }
        public string Provider { get; set; }
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public Guid QuoteId { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public GetQuoteResponse Quote { get; set; }
        public GetCustomerWithImageResponse Customer { get; set; }
    }
}
