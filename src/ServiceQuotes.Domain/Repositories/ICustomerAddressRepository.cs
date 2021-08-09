using ServiceQuotes.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ServiceQuotes.Domain.Repositories
{
    public interface ICustomerAddressRepository : IRepository<CustomerAddress>
    {
        Task<CustomerAddress> GetWithCustomerAndAddress(Guid customerId, Guid addressId);
        Task<CustomerAddress> GetByCustomerIdAndName(Guid customerId, string name);
        Task<IEnumerable<CustomerAddress>> FindWithAddress(Guid customerId, Expression<Func<CustomerAddress, bool>> predicate);
        Task<List<String>> GetCities(Guid customerId);
    }
}
