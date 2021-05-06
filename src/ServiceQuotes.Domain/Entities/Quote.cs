//using ServiceQuotes.Domain.Entities.Abstractions;
//using System;
//using System.Collections.Generic;

namespace ServiceQuotes.Domain.Entities
{
    public class Quote : Entity
    {
        public int ReferenceNumber { get; set; }
        public decimal Total { get; set; }
        public Status Status { get; set; }
    }
}
