using System.Collections.Generic;

namespace ServiceQuotes.Application.DTOs.Paynow
{
    public class PaynowErrorResponse
    {
        public int StatusCode { get; set; }
        public IEnumerable<PaynowError> Errors { get; set; }
    }
}
