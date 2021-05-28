using ServiceQuotes.Domain.Entities.Abstractions;
using ServiceQuotes.Domain.Entities.Enums;
using System;
using System.Collections.Generic;

namespace ServiceQuotes.Domain.Entities
{
    public class Quote : Entity
    {
        public Quote()
        {
            Payments = new HashSet<Payment>();
        }

        public int ReferenceNumber { get; set; }
        public decimal Total { get; set; }
        public Status Status { get; set; }
        public Guid ServiceRequestId { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ServiceRequest ServiceRequest { get; set; }
    }
}
