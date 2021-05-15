namespace ServiceQuotes.Application.DTOs.Customer
{
    public class GetCustomerWithImageResponse : GetCustomerResponse
    {
        public byte[] Image { get; set; }
    }
}
