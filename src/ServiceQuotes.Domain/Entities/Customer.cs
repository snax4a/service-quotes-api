using ServiceQuotes.Domain.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ServiceQuotes.Domain.Entities
{
    public class Customer : Entity
    {
        public Customer()
        {
            CustomerAddresses = new HashSet<CustomerAddress>();
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

        public void AddAddress(Guid addressId)
        {
            var existingAddress = CustomerAddresses.FirstOrDefault(ca => ca.AddressId == addressId);
            if (existingAddress != null)
                return;

            CustomerAddresses.Add(new CustomerAddress
            {
                AddressId = addressId,
                CustomerId = this.Id
            });
        }

        public void RemoveAddress(Guid addressId)
        {
            var addressToRemove = CustomerAddresses.FirstOrDefault(ca => ca.AddressId == addressId);
            if (addressToRemove != null)
            {
                CustomerAddresses.Remove(addressToRemove);
            }
        }
    }
}
