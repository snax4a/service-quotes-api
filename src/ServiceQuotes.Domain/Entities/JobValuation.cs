using ServiceQuotes.Domain.Entities.Abstractions;
using System;
using System.Collections.Generic;

namespace ServiceQuotes.Domain.Entities
{
    public class JobValuation : Entity
    {
        public JobValuation()
        {
            ServiceRequestJobValuations = new HashSet<ServiceRequestJobValuation>();
        }

        public string WorkType { get; set; }
        public decimal HourlyRate { get; set; }
        public TimeSpan LaborHours { get; set; }

        public decimal TotalValue
        {
            get
            {
                return Math.Round((int)LaborHours.TotalMinutes * HourlyRate / 60m, 2);
            }
        }

        public virtual ICollection<ServiceRequestJobValuation> ServiceRequestJobValuations { get; set; }
    }
}
