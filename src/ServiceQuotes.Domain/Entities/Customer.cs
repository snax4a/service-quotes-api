using ServiceQuotes.Domain.Entities.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceQuotes.Domain.Entities
{
    public class Customer : Entity
    {
        public Customer()
        {
            CustomerAddresses = new HashSet<CustomerAddress>();
            Payments = new HashSet<Payment>();
            ServiceRequests = new HashSet<ServiceRequest>();
        }

        public Guid AccountId { get; set; }

        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; }

        [Required]
        [MaxLength(50)]
        public string CompanyName { get; set; }

        [Required]
        [MaxLength(12)]
        public string VatNumber { get; set; }

        public virtual ICollection<CustomerAddress> CustomerAddresses { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<ServiceRequest> ServiceRequests { get; set; }
    }
}
