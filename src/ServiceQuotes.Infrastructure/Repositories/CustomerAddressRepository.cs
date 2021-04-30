using Microsoft.EntityFrameworkCore;
using ServiceQuotes.Domain.Entities;
using ServiceQuotes.Domain.Repositories;
using ServiceQuotes.Infrastructure.Context;
using System;
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
    }
}
