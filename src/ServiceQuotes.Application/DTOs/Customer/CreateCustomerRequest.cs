using System;
using System.ComponentModel.DataAnnotations;

namespace ServiceQuotes.Application.DTOs.Customer
{
    public class CreateCustomerRequest
    {
        [Required(ErrorMessage = "Account is required")]
        public Guid AccountId { get; set; }

        [Required(ErrorMessage = "Company name is required")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Vat number is required")]
        public string VatNumber { get; set; }
    }
}
