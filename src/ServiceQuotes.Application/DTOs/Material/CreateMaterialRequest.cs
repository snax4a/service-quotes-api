using System;
using System.ComponentModel.DataAnnotations;

namespace ServiceQuotes.Application.DTOs.Material
{
    public class CreateMaterialRequest
    {
        [Required(ErrorMessage = "Service request id is required")]
        public Guid ServiceRequestId { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Range(1, 999)]
        [Required(ErrorMessage = "Quantity is required")]
        public int Quantity { get; set; }

        [Range(0.01, 9999999)]
        [Required(ErrorMessage = "Unit price is required")]
        public decimal UnitPrice { get; set; }
    }
}
