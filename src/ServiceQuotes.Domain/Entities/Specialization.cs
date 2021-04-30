using ServiceQuotes.Domain.Entities.Abstractions;
using System.Collections.Generic;

namespace ServiceQuotes.Domain.Entities
{
    public class Specialization : Entity
    {
        public Specialization()
        {
            EmployeeSpecializations = new HashSet<EmployeeSpecialization>();
        }

        public string Name { get; set; }
        public virtual ICollection<EmployeeSpecialization> EmployeeSpecializations { get; set; }
    }
}
