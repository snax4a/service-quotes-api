using System;

namespace ServiceQuotes.Domain.Entities
{
    public class ServiceRequestJobValuation
    {
        public Guid EmployeeId { get; set; }
        public Guid JobValuationId { get; set; }
        public Guid ServiceRequestId { get; set; }
        public DateTime Date { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual JobValuation JobValuation { get; set; }
        public virtual ServiceRequest ServiceRequest { get; set; }
    }
}
