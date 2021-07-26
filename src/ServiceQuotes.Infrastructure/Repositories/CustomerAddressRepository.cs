using Microsoft.EntityFrameworkCore;
using ServiceQuotes.Domain.Entities;
using ServiceQuotes.Domain.Repositories;
using ServiceQuotes.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ServiceQuotes.Infrastructure.Repositories
{
    public class CustomerAddressRepository : Repository<CustomerAddress>, ICustomerAddressRepository
    {
        public CustomerAddressRepository(AppDbContext dbContext) : base(dbContext) { }

        public async Task<CustomerAddress> GetWithCustomerAndAddress(Guid customerId, Guid addressId)
        {
            return await _entities
                        .Include(ca => ca.Customer)
                        .Include(ca => ca.Address)
                        .SingleOrDefaultAsync(ca => ca.CustomerId == customerId && ca.AddressId == addressId);

        }

        public async Task<CustomerAddress> GetByCustomerIdAndName(Guid customerId, string name)
        {
            return await _entities
                        .Include(ca => ca.Customer)
                        .Include(ca => ca.Address)
                        .SingleOrDefaultAsync(ca => ca.CustomerId == customerId && ca.Name == name);
        }

        public async Task<IEnumerable<CustomerAddress>> FindWithAddress(Guid customerId, Expression<Func<CustomerAddress, bool>> predicate)
        {
            return await _entities
                        .Include(ca => ca.Address)
                        .Where(ca => ca.CustomerId == customerId)
                        .Where(predicate)
                        .ToListAsync();
        }
    }
}
