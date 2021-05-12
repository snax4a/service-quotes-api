using System;
using System.Collections.Generic;

namespace ServiceQuotes.Domain.Entities
{
    public class CustomerAddress
    {
        public CustomerAddress()
        {
            ServiceRequests = new HashSet<ServiceRequest>();
        }

        public Guid CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public Guid AddressId { get; set; }
        public virtual Address Address { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ServiceRequest> ServiceRequests { get; set; }
    }
}
