using System;

namespace ServiceQuotes.Domain.Entities
{
    public class EmployeeSpecialization
    {
        public Guid EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }

        public Guid SpecializationId { get; set; }
        public virtual Specialization Specialization { get; set; }
    }
}
