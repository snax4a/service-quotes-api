using ServiceQuotes.Domain.Entities.Enums;
using System;

namespace ServiceQuotes.Application.DTOs.Account
{
    public class GetAccountDTO
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}
