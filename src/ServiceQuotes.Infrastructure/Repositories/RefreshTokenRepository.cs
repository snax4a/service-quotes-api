using Microsoft.EntityFrameworkCore;
using ServiceQuotes.Domain.Entities;
using ServiceQuotes.Domain.Repositories;
using ServiceQuotes.Infrastructure.Context;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceQuotes.Infrastructure.Repositories
{
    public class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(AppDbContext dbContext) : base(dbContext) { }

        public async Task<RefreshToken> GetByToken(string token)
        {
            return await _entities.SingleOrDefaultAsync(rt => rt.Token == token);
        }
    }
}
