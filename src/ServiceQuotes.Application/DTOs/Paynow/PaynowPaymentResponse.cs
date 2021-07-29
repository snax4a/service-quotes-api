using ServiceQuotes.Domain.Entities.Enums;

namespace ServiceQuotes.Application.DTOs.Paynow
{
    public class PaynowPaymentResponse
    {
        public string RedirectUrl { get; set; }
        public string PaymentId { get; set; }
        public Status Status { get; set; }
    }
}
