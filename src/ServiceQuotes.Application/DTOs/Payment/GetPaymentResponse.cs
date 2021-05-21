using ServiceQuotes.Domain.Entities.Enums;
using System;

namespace ServiceQuotes.Application.DTOs.Payment
{
    public class GetPaymentResponse
    {
        public Guid Id { get; set; }
        public string Provider { get; set; }
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
        public Status Status { get; set; }
        public Guid QuoteId { get; set; }
        public Guid CutomerId { get; set; }
        public DateTime Date { get; set; }
    }
}
