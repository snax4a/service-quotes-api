using Microsoft.EntityFrameworkCore;
using ServiceQuotes.Domain.Entities;
using ServiceQuotes.Domain.Repositories;
using ServiceQuotes.Infrastructure.Context;
using System;
using System.Threading.Tasks;

namespace ServiceQuotes.Infrastructure.Repositories
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(AppDbContext dbContext) : base(dbContext) { }

        public async Task<Customer> GetByCompanyName(string companyName)
        {
            return await _entities.SingleOrDefaultAsync(c => c.CompanyName == companyName);
        }

        public async Task<Customer> GetWithAddresses(Guid id)
        {
            return await _entities
                        .Include(c => c.CustomerAddresses)
                        .ThenInclude(ca => ca.Address)
                        .SingleOrDefaultAsync(c => c.Id == id);
        }
    }
}
