using ServiceQuotes.Application.DTOs.Employee;
using System;

namespace ServiceQuotes.Application.DTOs.JobValuation
{
    public class GetServiceRequestJobValuationResponse
    {
        public GetEmployeeWithAccountImageResponse Employee { get; set; }
        public GetJobValuationResponse JobValuation { get; set; }
        public Guid ServiceRequestId { get; set; }
        public DateTime Date { get; set; }
    }
}
