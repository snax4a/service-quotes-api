using System.ComponentModel.DataAnnotations;

namespace ServiceQuotes.Application.DTOs.Account
{
    public class RevokeTokenRequest
    {
        [Required(ErrorMessage = "Token is required")]
        public string Token { get; set; }
    }
}
