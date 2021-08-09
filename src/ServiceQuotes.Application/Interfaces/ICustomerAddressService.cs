using ServiceQuotes.Application.DTOs.CustomerAddress;
using ServiceQuotes.Application.Filters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceQuotes.Application.Interfaces
{
    public interface ICustomerAddressService : IDisposable
    {
        Task<List<GetCustomerAddressResponse>> GetCustomerAddresses(Guid customerId, GetCustomerAddressFilter filter);

        Task<List<String>> GetCities(Guid customerId);
    }
}
