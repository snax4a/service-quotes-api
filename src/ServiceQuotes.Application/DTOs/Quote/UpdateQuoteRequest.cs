using ServiceQuotes.Domain.Entities.Enums;

namespace ServiceQuotes.Application.DTOs.Quote
{
    public class UpdateQuoteStatusRequest
    {
        public Status Status { get; set; }
    }
}
