using Microsoft.EntityFrameworkCore;
using ServiceQuotes.Domain.Entities;
using ServiceQuotes.Domain.Repositories;
using ServiceQuotes.Infrastructure.Context;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceQuotes.Infrastructure.Repositories
{
    public class AccountRepository : Repository<Account>, IAccountRepository
    {
        public AccountRepository(AppDbContext dbContext) : base(dbContext) { }

        public async Task<Account> GetByEmail(string email)
        {
            return await _entities.SingleOrDefaultAsync(a => a.Email == email);
        }

        public async Task<Account> GetByRefreshToken(string token)
        {
            return await _entities
                        .Include(a => a.RefreshTokens)
                        .SingleOrDefaultAsync(a => a.RefreshTokens.Any(t => t.Token == token));
        }
    }
}
