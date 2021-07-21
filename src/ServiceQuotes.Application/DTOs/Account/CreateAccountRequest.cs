using ServiceQuotes.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceQuotes.Application.DTOs.Account
{
    public class CreateAccountRequest
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(30, ErrorMessage = "Must be between 6 and 30 characters", MinimumLength = 6)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Repeat password is required")]
        [Compare("Password")]
        public string RepeatPassword { get; set; }

        public byte[] Image { get; set; }

        [Required(ErrorMessage = "Role is required")]
        [EnumDataType(typeof(Role))]
        public Role Role { get; set; }

        [StringLength(30, ErrorMessage = "Must be between 3 and 30 characters", MinimumLength = 3)]
        public string CompanyName { get; set; }

        [StringLength(20, ErrorMessage = "Must be between 10 and 20 characters", MinimumLength = 10)]
        public string VatNumber { get; set; }

        [StringLength(30, ErrorMessage = "Must be between 3 and 30 characters", MinimumLength = 3)]
        public string FirstName { get; set; }

        [StringLength(30, ErrorMessage = "Must be between 3 and 30 characters", MinimumLength = 3)]
        public string LastName { get; set; }
    }
}
