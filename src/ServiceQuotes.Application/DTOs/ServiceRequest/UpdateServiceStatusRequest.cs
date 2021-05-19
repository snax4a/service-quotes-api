using ServiceQuotes.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceQuotes.Application.DTOs.ServiceRequest
{
    public class UpdateServiceStatusRequest
    {
        [Required(ErrorMessage = "Status is required")]
        public Status Status { get; set; }
    }
}
