using ServiceQuotes.Domain.Entities.Abstractions;
using ServiceQuotes.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ServiceQuotes.Domain.Entities
{
    public class Account : Entity
    {
        public Account()
        {
            RefreshTokens = new HashSet<RefreshToken>();
        }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }
        public Role Role { get; set; }
        public byte[] Image { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }

        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}
