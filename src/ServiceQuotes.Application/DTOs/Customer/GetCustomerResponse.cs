using ServiceQuotes.Application.DTOs.CustomerAddress;
using System;
using System.Collections.Generic;

namespace ServiceQuotes.Application.DTOs.Customer
{
    public class GetCustomerResponse
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public string CompanyName { get; set; }
        public string VatNumber { get; set; }
    }
}
