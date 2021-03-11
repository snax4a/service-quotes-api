using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceQuotes.Domain.Entities
{
    public class CustomerAddress
    {
        public Guid CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public Guid AddressId { get; set; }
        public virtual Address Address { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
