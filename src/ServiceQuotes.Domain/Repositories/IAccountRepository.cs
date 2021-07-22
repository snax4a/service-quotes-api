using ServiceQuotes.Domain.Entities;
using System.Threading.Tasks;

namespace ServiceQuotes.Domain.Repositories
{
    public interface IAccountRepository : IRepository<Account>
    {
        Task<Account> GetByEmail(string email);
    }
}
