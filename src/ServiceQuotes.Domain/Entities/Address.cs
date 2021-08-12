using ServiceQuotes.Domain.Entities.Abstractions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ServiceQuotes.Domain.Entities
{
    public class Address : Entity
    {
        public Address()
        {
            CustomerAddresses = new HashSet<CustomerAddress>();
            ServiceRequests = new HashSet<ServiceRequest>();
        }

        [Required]
        [MaxLength(30)]
        public string City { get; set; }

        [Required]
        [MaxLength(20)]
        public string ZipCode { get; set; }

        [Required]
        [MaxLength(100)]
        public string Street { get; set; }

        [Phone]
        [Required]
        [MaxLength(11)]
        public string PhoneNumber { get; set; }

        [JsonIgnore]
        public virtual ICollection<CustomerAddress> CustomerAddresses { get; set; }
        public virtual ICollection<ServiceRequest> ServiceRequests { get; set; }
    }
}
