using ServiceQuotes.Domain.Entities.Enums;

namespace ServiceQuotes.Application.Filters
{
    public class GetServiceRequestsFilter : SearchStringFilter
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string DateRange { get; set; }
        public Status? Status { get; set; }
        public string EmployeeId { get; set; }
        public string CustomerId { get; set; }
    }
}
