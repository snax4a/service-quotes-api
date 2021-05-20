using ServiceQuotes.Application.DTOs.Quote;
using ServiceQuotes.Application.Filters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceQuotes.Application.Interfaces
{
    public interface IQuoteService : IDisposable
    {
        Task<List<GetQuoteResponse>> GetAllQuotes(GetQuotesFilter filter);

        Task<GetQuoteResponse> GetQuoteById(Guid id);

        Task<GetQuoteResponse> UpdateQuoteStatus(Guid id, UpdateQuoteStatusRequest dto);
    }
}
