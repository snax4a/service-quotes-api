using System;

namespace ServiceQuotes.Application.DTOs.JobValuation
{
    public class GetJobValuationResponse
    {
        public Guid Id { get; set; }
        public string WorkType { get; set; }
        public decimal HourlyRate { get; set; }
        public TimeSpan LaborHours { get; set; }
    }
}
