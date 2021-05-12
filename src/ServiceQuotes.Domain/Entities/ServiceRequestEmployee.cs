using System;

namespace ServiceQuotes.Domain.Entities
{
    public class ServiceRequestEmployee
    {
        public Guid ServiceRequestId { get; set; }
        public virtual ServiceRequest ServiceRequest { get; set; }
        public Guid EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
