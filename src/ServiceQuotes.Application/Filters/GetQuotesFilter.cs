using ServiceQuotes.Domain.Entities.Enums;
using System;

namespace ServiceQuotes.Application.Filters
{
    public class GetQuotesFilter : SearchStringFilter
    {
        public Guid? CustomerId { get; set; }
        public int? ReferenceNumber { get; set; }
        public Status? Status { get; set; }
        public int Limit { get; set; }
    }
}
