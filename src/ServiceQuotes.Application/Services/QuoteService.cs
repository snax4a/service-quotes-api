using AutoMapper;
using LinqKit;
using ServiceQuotes.Application.DTOs.Quote;
using ServiceQuotes.Application.Exceptions;
using ServiceQuotes.Application.Filters;
using ServiceQuotes.Application.Interfaces;
using ServiceQuotes.Domain;
using ServiceQuotes.Domain.Entities;
using ServiceQuotes.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceQuotes.Application.Services
{
    public class QuoteService : IQuoteService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public QuoteService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<GetQuoteResponse>> GetAllQuotes(GetQuotesFilter filter)
        {
            // prepare filter predicate
            var predicate = PredicateBuilder.New<Quote>(true);

            if (!string.IsNullOrEmpty(filter?.SearchString))
            {
                predicate = predicate.Or(p => p.ReferenceNumber.Equals(filter.SearchString.ToLower()));
                predicate = predicate.Or(p => p.Status.Equals(filter.SearchString.ToLower()));
            }

            var quote = await _unitOfWork.Quotes.Find(predicate);

            return _mapper.Map<List<GetQuoteResponse>>(quote);
        }

        public async Task<GetQuoteResponse> GetQuoteById(Guid id)
        {
            return _mapper.Map<GetQuoteResponse>(await _unitOfWork.Quotes.Get(id));
        }

        public async Task<GetQuoteResponse> UpdateQuoteStatus(Guid id, UpdateQuoteStatusRequest dto)
        {
            var quote = await _unitOfWork.Quotes.Get(id);

            // validate
            if (quote is null) throw new KeyNotFoundException();

            if (quote.Status != dto.Status)
                quote.Status = dto.Status;

            _unitOfWork.Commit();
            return _mapper.Map<GetQuoteResponse>(quote);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _unitOfWork.Dispose();
            }
        }
    }
}
