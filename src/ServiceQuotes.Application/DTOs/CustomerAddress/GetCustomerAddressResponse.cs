namespace ServiceQuotes.Application.DTOs.CustomerAddress
{
    public class GetCustomerAddressResponse
    {
        public string Name { get; set; }
        public GetAddressResponse Address { get; set; }
    }
}
