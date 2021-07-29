using System.ComponentModel.DataAnnotations;

namespace ServiceQuotes.Application.DTOs.Paynow
{
    public class Buyer
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
    }
}
