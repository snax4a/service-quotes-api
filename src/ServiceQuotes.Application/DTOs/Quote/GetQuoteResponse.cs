using System;

namespace ServiceQuotes.Application.DTOs.Quote
{
    public class GetQuoteResponse
    {
        public Guid Id { get; set; }
        public int ReferenceNumber { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; }
        public Guid ServiceRequestId { get; set; }
        public DateTime Created { get; set; }
    }
}
