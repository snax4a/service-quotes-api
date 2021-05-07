using ServiceQuotes.Domain.Entities.Enums;

namespace ServiceQuotes.Application.Filters
{
    public class GetQuotesFilter : SearchStringFilter
    {
        public int ReferenceNumber { get; set; }
        public Status? Status { get; set; }
    }
}
