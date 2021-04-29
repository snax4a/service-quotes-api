using System;

namespace ServiceQuotes.Application.DTOs.Account
{
    public class GetAccountResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}
