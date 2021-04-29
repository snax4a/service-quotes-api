using ServiceQuotes.Application.DTOs.CustomerAddress;
using System;
using System.Collections.Generic;

namespace ServiceQuotes.Application.DTOs.Customer
{
    public class GetCustomerWithAddressesResponse
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public string CompanyName { get; set; }
        public string VatNumber { get; set; }
        public ICollection<GetCustomerAddressResponse> CustomerAddresses { get; set; }
    }
}
