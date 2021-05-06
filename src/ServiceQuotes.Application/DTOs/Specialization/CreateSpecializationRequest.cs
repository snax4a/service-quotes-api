using System;
using System.ComponentModel.DataAnnotations;

namespace ServiceQuotes.Application.DTOs.Specialization
{
    public class CreateSpecializationRequest
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
    }
}
