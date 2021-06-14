using ServiceQuotes.Application.Helpers;

namespace ServiceQuotes.Application.DTOs.JobValuation
{
    public class RemoveJobValuationRequest
    {
        [ValidateGuid]
        public string EmployeeId { get; set; }

        [ValidateGuid]
        public string JobValuationId { get; set; }

        [ValidateGuid]
        public string ServiceRequestId { get; set; }

    }
}
