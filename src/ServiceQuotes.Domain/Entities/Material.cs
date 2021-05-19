using ServiceQuotes.Domain.Entities.Abstractions;
using System;

namespace ServiceQuotes.Domain.Entities
{
    public class Material : Entity
    {
        public Guid ServiceRequestId { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public virtual ServiceRequest ServiceRequest { get; set; }
    }
}
