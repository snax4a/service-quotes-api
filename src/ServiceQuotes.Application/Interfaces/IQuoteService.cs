using ServiceQuotes.Application.DTOs.Quote;
using ServiceQuotes.Application.Filters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceQuotes.Application.Interfaces
{
    public interface IQuoteService : IDisposable
    {
        Task<List<GetQuoteWithServiceDetailsResponse>> GetAllQuotes(GetQuotesFilter filter);

        Task<GetQuoteWithServiceDetailsResponse> GetQuoteById(Guid id);
        Task<List<GetQuoteWithServiceDetailsResponse>> GetTopUnpaidQuotes(GetQuotesFilter filter);

        Task<GetQuoteResponse> UpdateQuoteStatus(Guid id, UpdateQuoteStatusRequest dto);
    }
}
