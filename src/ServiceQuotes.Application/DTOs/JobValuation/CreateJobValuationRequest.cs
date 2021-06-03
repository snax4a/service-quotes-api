using ServiceQuotes.Application.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace ServiceQuotes.Application.DTOs.JobValuation
{
    public class CreateJobValuationRequest
    {
        [ValidateGuid]
        [Required(ErrorMessage = "EmployeeId is required")]
        public string EmployeeId { get; set; }

        [Required(ErrorMessage = "Work type is required")]
        public string WorkType { get; set; }

        [Range(0.01, 9999999)]
        [Required(ErrorMessage = "Hourly rate is required")]
        public decimal HourlyRate { get; set; }

        [Required(ErrorMessage = "Labor hours is required")]
        public TimeSpan LaborHours { get; set; }
    }
}
