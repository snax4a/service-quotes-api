using ServiceQuotes.Domain.Entities.Abstractions;
using System;
using System.Net;

namespace ServiceQuotes.Domain.Entities
{
    public class RefreshToken : Entity
    {
        public Guid AccountId { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public DateTime Created { get; set; }
        public IPAddress CreatedByIp { get; set; }
        public DateTime? Revoked { get; set; }
        public IPAddress RevokedByIp { get; set; }
        public string ReplacedByToken { get; set; }
        public bool IsActive => Revoked == null && !IsExpired;

        public virtual Account Account { get; set; }
    }
}
