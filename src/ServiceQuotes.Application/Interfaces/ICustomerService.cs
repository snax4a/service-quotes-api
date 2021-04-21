using ServiceQuotes.Application.DTOs.Customer;
using ServiceQuotes.Application.Filters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceQuotes.Application.Interfaces
{
    public interface ICustomerService : IDisposable
    {
        Task<List<GetCustomerDTO>> GetAllCustomers(GetCustomersFilter filter);

        Task<GetCustomerDTO> GetCustomerById(Guid id);

        Task<GetCustomerDTO> CreateCustomer(CreateCustomerDTO customer);

        Task<GetCustomerDTO> UpdateCustomer(Guid id, UpdateCustomerDTO updatedCustomer);
    }
}
