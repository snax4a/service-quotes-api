using ServiceQuotes.Domain.Entities.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace ServiceQuotes.Application.DTOs.Paynow
{
    public class CreatePaynowPaymentRequest
    {
        [Required(ErrorMessage = "Amount is required")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Currency is required")]
        public Currency Currency { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "ExternalId is required")]
        public Guid ExternalId { get; set; }

        [Required(ErrorMessage = "Buyer is required")]
        public Buyer Buyer { get; set; }
    }
}
