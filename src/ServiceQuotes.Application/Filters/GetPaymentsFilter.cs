using ServiceQuotes.Domain.Entities.Enums;

namespace ServiceQuotes.Application.Filters
{
    public class GetPaymentsFilter : SearchStringFilter
    {
        public string Provider { get; set; }
        public string TransactionId { get; set; }
        public string DateRange { get; set; }
        public Status? Status { get; set; }
    }
}
