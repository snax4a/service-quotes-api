using ServiceQuotes.Domain.Entities.Abstractions;
using ServiceQuotes.Domain.Entities.Enums;
using System;

namespace ServiceQuotes.Domain.Entities
{
    public class Payment : Entity
    {
        public string Provider { get; set; }
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
        public Status Status { get; set; }
        public Guid QuoteId { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }

        public virtual Quote Quote { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
