using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ServiceQuotes.Application.DTOs.Paynow
{
    public class CreatePaynowPaymentRequest
    {
        [Required(ErrorMessage = "Amount is required")]
        [Range(100, Int32.MaxValue)]
        [JsonPropertyName("amount")]
        public int Amount { get; set; }

        [Required(ErrorMessage = "Currency is required")]
        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "ExternalId is required")]
        [JsonPropertyName("externalId")]
        public Guid ExternalId { get; set; }

        [Required(ErrorMessage = "Buyer is required")]
        [JsonPropertyName("buyer")]
        public Buyer Buyer { get; set; }
    }
}
