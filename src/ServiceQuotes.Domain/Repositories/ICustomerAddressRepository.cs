using ServiceQuotes.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace ServiceQuotes.Domain.Repositories
{
    public interface ICustomerAddressRepository : IRepository<CustomerAddress>
    {
        Task<CustomerAddress> GetWithCustomerAndAddress(Guid customerId, Guid addressId);
        Task<CustomerAddress> GetByCustomerIdAndName(Guid customerId, string name);
    }
}
