using ServiceQuotes.Domain.Core.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ServiceQuotes.Domain.Entities
{
    public class Address : Entity
    {
        public Address()
        {
            CustomerAddresses = new HashSet<CustomerAddress>();
        }

        [Required]
        [MaxLength(30)]
        public string City { get; set; }

        [Required]
        [MaxLength(20)]
        public string ZipCode { get; set; }

        [Required]
        [MaxLength(30)]
        public string Street { get; set; }

        [Required]
        [MaxLength(5)]
        public string BuildingNumber { get; set; }

        [MaxLength(5)]
        public string ApartmentNumber { get; set; }

        [Phone]
        [Required]
        [MaxLength(11)]
        public string PhoneNumber { get; set; }

        public virtual ICollection<CustomerAddress> CustomerAddresses { get; set; }
    }
}
