using ServiceQuotes.Application.DTOs.Customer;
using ServiceQuotes.Application.DTOs.CustomerAddress;

namespace ServiceQuotes.Application.DTOs.ServiceRequest
{
    public class GetServiceWithCustomerAndAddressResponse : GetServiceResponse
    {
        public GetCustomerWithImageResponse Customer { get; set; }
        public GetAddressResponse Address { get; set; }
    }
}
