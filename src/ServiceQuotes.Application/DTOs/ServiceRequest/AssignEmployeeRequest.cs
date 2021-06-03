using ServiceQuotes.Application.Helpers;

namespace ServiceQuotes.Application.DTOs.ServiceRequest
{
    public class AssignEmployeeRequest
    {
        [ValidateGuid]
        public string EmployeeId { get; set; }
    }
}
