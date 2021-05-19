using System;

namespace ServiceQuotes.Application.DTOs.ServiceRequest
{
    public class GetServiceResponse
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid AddressId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime? PlannedExecutionDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public DateTime Created { get; set; }
    }
}
