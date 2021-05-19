using ServiceQuotes.Domain.Entities.Abstractions;
using ServiceQuotes.Domain.Entities.Enums;
using System;
using System.Collections.Generic;

namespace ServiceQuotes.Domain.Entities
{
    public class ServiceRequest : Entity
    {
        public ServiceRequest()
        {
            ServiceRequestEmployees = new HashSet<ServiceRequestEmployee>();
            Materials = new HashSet<Material>();
            ServiceRequestJobValuations = new HashSet<ServiceRequestJobValuation>();
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? PlannedExecutionDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public DateTime Created { get; set; }
        public Status Status { get; set; }
        public Guid CustomerId { get; set; }
        public Guid AddressId { get; set; }

        public virtual CustomerAddress CustomerAddress { get; set; }
        public virtual ICollection<ServiceRequestEmployee> ServiceRequestEmployees { get; set; }
        public virtual ICollection<Material> Materials { get; set; }
        public virtual ICollection<ServiceRequestJobValuation> ServiceRequestJobValuations { get; set; }
    }
}
