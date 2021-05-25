using ServiceQuotes.Application.Helpers;
using ServiceQuotes.Domain.Entities.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace ServiceQuotes.Application.DTOs.Payment
{
    public class CreatePaymentRequest
    {
        [Required(ErrorMessage = "Provider is required")]
        public string Provider { get; set; }

        [Required(ErrorMessage = "Transaction ID is required")]
        public string TransactionId { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public Status Status { get; set; }

        [ValidateGuid]
        public string QuoteId { get; set; }

        [ValidateGuid]
        public string CustomerId { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; }
    }
}
