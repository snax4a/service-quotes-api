using ServiceQuotes.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceQuotes.Application.DTOs.Account
{
    public class CreateAccountDTO
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Role is required")]
        [EnumDataType(typeof(Role))]
        public Role Role { get; set; }
    }
}
