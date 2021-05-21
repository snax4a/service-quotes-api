using ServiceQuotes.Domain.Entities.Enums;

namespace ServiceQuotes.Application.DTOs.Payment
{
    public class UpdatePaymentStatusRequest
    {
        public Status Status { get; set; }
    }
}
