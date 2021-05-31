using ServiceQuotes.Application.DTOs.ServiceRequest;

namespace ServiceQuotes.Application.DTOs.Quote
{
    public class GetQuoteWithServiceDetailsResponse : GetQuoteResponse
    {
        public GetServiceWithCustomerAndAddressResponse ServiceRequest { get; set; }
    }
}
