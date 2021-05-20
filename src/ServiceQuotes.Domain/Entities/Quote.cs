using ServiceQuotes.Domain.Entities.Abstractions;
using ServiceQuotes.Domain.Entities.Enums;
using System;
using System.Collections.Generic;

namespace ServiceQuotes.Domain.Entities
{
    public class Quote : Entity
    {
        public int ReferenceNumber { get; set; }
        public decimal Total { get; set; }
        public Status Status { get; set; }

        public virtual ICollection<Payment> Payment { get; set; }
    }
}
