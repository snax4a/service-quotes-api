using ServiceQuotes.Application.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace ServiceQuotes.Application.DTOs.ServiceRequest
{
    public class CreateServiceRequest
    {
        [ValidateGuid]
        public string CustomerId { get; set; }

        [ValidateGuid]
        public string AddressId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
    }
}
