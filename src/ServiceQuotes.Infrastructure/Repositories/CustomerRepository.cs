using Microsoft.EntityFrameworkCore;
using ServiceQuotes.Domain.Entities;
using ServiceQuotes.Domain.Repositories;
using ServiceQuotes.Infrastructure.Context;
using System.Threading.Tasks;

namespace ServiceQuotes.Infrastructure.Repositories
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(AppDbContext dbContext) : base(dbContext) { }

        public async Task<Customer> GetByCompanyName(string companyName)
        {
            return await DbSet
                        .AsNoTracking()
                        .SingleOrDefaultAsync(c => c.CompanyName == companyName);
        }
    }
}
