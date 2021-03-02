using System.ComponentModel.DataAnnotations;

namespace ServiceQuotes.Application.DTOs.Account
{
    public class AuthenticateDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
