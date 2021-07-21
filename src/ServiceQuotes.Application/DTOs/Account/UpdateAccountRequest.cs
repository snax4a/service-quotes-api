using ServiceQuotes.Application.Helpers;
using System.ComponentModel.DataAnnotations;

namespace ServiceQuotes.Application.DTOs.Account
{
    public class UpdateAccountRequest
    {
        public string Email { get; set; }
        public byte[] Image { get; set; }

        [NullableStringLength(6, 30, "Must be between 6 and 30 characters")]
        public string Password { get; set; }

        [Compare("Password")]
        public string RepeatPassword { get; set; }

        [NullableStringLength(3, 30, "Must be between 3 and 30 characters")]
        public string CompanyName { get; set; }

        [NullableStringLength(10, 20, "Must be between 10 and 20 characters")]
        public string VatNumber { get; set; }

        [NullableStringLength(3, 30, "Must be between 3 and 30 characters")]
        public string FirstName { get; set; }

        [NullableStringLength(3, 30, "Must be between 3 and 30 characters")]
        public string LastName { get; set; }
    }
}
