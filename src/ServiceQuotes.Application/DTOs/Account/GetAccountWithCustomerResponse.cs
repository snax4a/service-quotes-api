using System;

namespace ServiceQuotes.Application.DTOs.Account
{
    public class GetAccountWithCustomerResponse : GetAccountResponse
    {
        public Guid CustomerId { get; set; }
        public string CompanyName { get; set; }
        public string VatNumber { get; set; }
    }
}
