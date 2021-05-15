using ServiceQuotes.Domain.Entities.Abstractions;
using System;
using System.Collections.Generic;

namespace ServiceQuotes.Domain.Entities
{
    public class Employee : Entity
    {
        public Employee()
        {
            EmployeeSpecializations = new HashSet<EmployeeSpecialization>();
            ServiceRequestEmployees = new HashSet<ServiceRequestEmployee>();
            ServiceRequestJobValuations = new HashSet<ServiceRequestJobValuation>();
        }

        public Guid AccountId { get; set; }
        public virtual Account Account { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual ICollection<EmployeeSpecialization> EmployeeSpecializations { get; set; }
        public virtual ICollection<ServiceRequestEmployee> ServiceRequestEmployees { get; set; }
        public virtual ICollection<ServiceRequestJobValuation> ServiceRequestJobValuations { get; set; }
    }
}
