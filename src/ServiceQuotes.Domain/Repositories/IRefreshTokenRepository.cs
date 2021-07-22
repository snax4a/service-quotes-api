using ServiceQuotes.Domain.Entities;
using System.Threading.Tasks;

namespace ServiceQuotes.Domain.Repositories
{
    public interface IRefreshTokenRepository : IRepository<RefreshToken>
    {
        Task<RefreshToken> GetByToken(string token);
    }
}
