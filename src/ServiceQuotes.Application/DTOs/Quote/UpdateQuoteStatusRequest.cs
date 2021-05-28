using ServiceQuotes.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceQuotes.Application.DTOs.Quote
{
    public class UpdateQuoteStatusRequest
    {
        [Required(ErrorMessage = "Status is required")]
        public Status Status { get; set; }
    }
}
