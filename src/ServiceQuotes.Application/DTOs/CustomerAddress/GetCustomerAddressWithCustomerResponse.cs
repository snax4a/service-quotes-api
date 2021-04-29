using ServiceQuotes.Application.DTOs.Customer;

namespace ServiceQuotes.Application.DTOs.CustomerAddress
{
    public class GetCustomerAddressWithCustomerResponse
    {
        public string Name { get; set; }
        public GetCustomerResponse Customer { get; set; }
        public GetAddressResponse Address { get; set; }
    }
}
