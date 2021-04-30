using ServiceQuotes.Application.DTOs.Customer;
using ServiceQuotes.Application.DTOs.CustomerAddress;
using ServiceQuotes.Application.Filters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceQuotes.Application.Interfaces
{
    public interface ICustomerService : IDisposable
    {
        Task<List<GetCustomerResponse>> GetAllCustomers(GetCustomersFilter filter);

        Task<GetCustomerWithAddressesResponse> GetCustomerById(Guid id);

        Task<GetCustomerResponse> CreateCustomer(CreateCustomerRequest customer);

        Task<GetCustomerResponse> UpdateCustomer(Guid id, UpdateCustomerRequest updatedCustomer);

        Task<GetCustomerAddressWithCustomerResponse> GetAddressById(Guid customerId, Guid addressId);

        Task<GetCustomerAddressWithCustomerResponse> CreateAddress(Guid customerId, CreateAddressRequest address);
    }
}
