using System;
using System.ComponentModel.DataAnnotations;

namespace ServiceQuotes.Application.DTOs.Employee
{
    public class AssignSpecializationRequest
    {
        [Required(ErrorMessage = "Specialization id is required")]
        public Guid SpecializationId { get; set; }
    }
}
