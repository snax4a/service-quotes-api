using ServiceQuotes.Domain.Entities.Abstractions;
using ServiceQuotes.Domain.Entities.Enums;
using System;
using System.Collections.Generic;

namespace ServiceQuotes.Domain.Entities
{
    public class Payment : Entity
    {
        public Guid PaymentId { get; set; }
        public string Provider { get; set; }
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
        public Status Status { get; set; }
        public virtual Quote Quote { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual DateTime Date { get; set; }

        public virtual ICollection<Quote> Quotes { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
    }
}