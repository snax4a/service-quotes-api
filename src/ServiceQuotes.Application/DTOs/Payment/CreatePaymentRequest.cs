using ServiceQuotes.Application.Helpers;
using System.ComponentModel.DataAnnotations;

namespace ServiceQuotes.Application.DTOs.Payment
{
    public class CreatePaymentRequest
    {
        [ValidateGuid]
        public string QuoteId { get; set; }

        [Required(ErrorMessage = "Payment provider is required")]
        public string Provider { get; set; }
    }
}
