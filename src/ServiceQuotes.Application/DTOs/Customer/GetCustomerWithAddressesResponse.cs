using ServiceQuotes.Application.DTOs.CustomerAddress;
using System;
using System.Collections.Generic;

namespace ServiceQuotes.Application.DTOs.Customer
{
    public class GetCustomerWithAddressesResponse : GetCustomerWithImageResponse
    {
        public ICollection<GetCustomerAddressResponse> CustomerAddresses { get; set; }
    }
}
