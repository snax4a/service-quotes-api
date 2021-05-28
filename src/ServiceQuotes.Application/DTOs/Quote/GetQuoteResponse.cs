using ServiceQuotes.Domain.Entities.Enums;
using System;

namespace ServiceQuotes.Application.DTOs.Quote
{
    public class GetQuoteResponse
    {
        public Guid Id { get; set; }
        public int ReferenceNumber { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; }
    }
}
