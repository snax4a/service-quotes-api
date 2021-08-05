using ServiceQuotes.Application.Helpers;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ServiceQuotes.Application.DTOs.Paynow
{
    public class PaynowNotificationRequest
    {
        [Required(ErrorMessage = "PaymentId is required")]
        [JsonPropertyName("paymentId")]
        public string PaymentId { get; set; }

        [Required(ErrorMessage = "ExternalId is required")]
        [JsonPropertyName("externalId")]
        [ValidateGuid]
        public string ExternalId { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [Required(ErrorMessage = "ModifiedAt is required")]
        [JsonPropertyName("modifiedAt")]
        public DateTime ModifiedAt { get; set; }
    }
}
