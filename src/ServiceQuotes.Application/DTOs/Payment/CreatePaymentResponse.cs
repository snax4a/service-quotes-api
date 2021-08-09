namespace ServiceQuotes.Application.DTOs.Payment
{
    public class CreatePaymentResponse
    {
        public string RedirectUrl { get; set; }
        public GetPaymentResponse Payment { get; set; }
    }
}
