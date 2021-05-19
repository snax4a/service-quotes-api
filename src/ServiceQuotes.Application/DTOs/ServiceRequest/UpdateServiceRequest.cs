using System;

namespace ServiceQuotes.Application.DTOs.ServiceRequest
{
    public class UpdateServiceRequest
    {
        public Guid? CustomerId { get; set; }
        public Guid? AddressId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? PlannedExecutionDate { get; set; }
    }
}
