using ServiceQuotes.Domain.Core.Entities;
using ServiceQuotes.Domain.Entities.Enums;
using System;
using System.Collections.Generic;

namespace ServiceQuotes.Domain.Entities
{
    public class Account : Entity
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public Role Role { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; }

        public bool OwnsToken(string token)
        {
            return this.RefreshTokens?.Find(x => x.Token == token) != null;
        }
    }
}
