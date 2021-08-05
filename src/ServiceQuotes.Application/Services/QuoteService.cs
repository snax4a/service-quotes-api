using AutoMapper;
using LinqKit;
using ServiceQuotes.Application.DTOs.Customer;
using ServiceQuotes.Application.DTOs.CustomerAddress;
using ServiceQuotes.Application.DTOs.Quote;
using ServiceQuotes.Application.DTOs.ServiceRequest;
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

        public async Task<List<GetQuoteWithServiceDetailsResponse>> GetAllQuotes(GetQuotesFilter filter)
        {
            // prepare filter predicate
            var predicate = PredicateBuilder.New<Quote>(true);

            if (!string.IsNullOrEmpty(filter?.SearchString))
            {
                predicate = predicate.Or(p => p.ServiceRequest.Title.ToLower().Contains(filter.SearchString.ToLower()));
                predicate = predicate.Or(p => p.ServiceRequest.Customer.CompanyName.ToLower().Contains(filter.SearchString.ToLower()));
                predicate = predicate.Or(p => p.ServiceRequest.Address.Street.ToLower().Contains(filter.SearchString.ToLower()));
                predicate = predicate.Or(p => p.ServiceRequest.Address.City.ToLower().Contains(filter.SearchString.ToLower()));
            }

            if (!string.IsNullOrEmpty(filter?.DateRange))
            {
                switch (filter.DateRange)
                {
                    case "30-days":
                        predicate = predicate.And(p => p.Created >= DateTime.UtcNow.AddDays(-30));
                        break;
                    case "7-days":
                        predicate = predicate.And(p => p.Created >= DateTime.UtcNow.AddDays(-7));
                        break;
                    case "today":
                        predicate = predicate.And(p => p.Created.Date == DateTime.UtcNow.Date);
                        break;
                }
            }

            if (filter.Status is not null)
            {
                predicate = predicate.And(p => p.Status.Equals(filter.Status));
            }

            if (filter?.CustomerId is not null && filter?.CustomerId != Guid.Empty)
            {
                predicate = predicate.And(p => p.ServiceRequest.CustomerId == filter.CustomerId);
            }

            var quotes = await _unitOfWork.Quotes.FindWithServiceDetails(predicate);

            return quotes.Select(q => new GetQuoteWithServiceDetailsResponse()
            {
                Id = q.Id,
                ReferenceNumber = q.ReferenceNumber,
                Total = q.Total,
                Status = q.Status.ToString(),
                Created = q.Created,
                ServiceRequest = new GetServiceWithCustomerAndAddressResponse()
                {
                    Id = q.ServiceRequest.Id,
                    CustomerId = q.ServiceRequest.CustomerId,
                    AddressId = q.ServiceRequest.AddressId,
                    Title = q.ServiceRequest.Title,
                    Description = q.ServiceRequest.Description,
                    Status = q.ServiceRequest.Status.ToString(),
                    PlannedExecutionDate = q.ServiceRequest.PlannedExecutionDate,
                    CompletionDate = q.ServiceRequest.CompletionDate,
                    Created = q.ServiceRequest.Created,
                    Address = _mapper.Map<GetAddressResponse>(q.ServiceRequest.Address),
                    Customer = new GetCustomerWithImageResponse()
                    {
                        Id = q.ServiceRequest.Customer.Id,
                        AccountId = q.ServiceRequest.Customer.AccountId,
                        CompanyName = q.ServiceRequest.Customer.CompanyName,
                        VatNumber = q.ServiceRequest.Customer.VatNumber,
                        Image = q.ServiceRequest.Customer.Account.Image,
                    }
                }
            }).ToList();
        }

        public async Task<List<GetQuoteWithServiceDetailsResponse>> GetTopUnpaidQuotes(GetQuotesFilter filter)
        {
            IEnumerable<Quote> quotes;
            int limit = filter.Limit != 0 ? filter.Limit : 3;

            if (filter?.CustomerId is not null && filter?.CustomerId != Guid.Empty)
            {
                quotes = await _unitOfWork.Quotes.GetTopQuotesByCustomerIdAndStatus((Guid)filter?.CustomerId, Status.Unpaid, limit);
            }
            else
            {
                quotes = await _unitOfWork.Quotes.GetTopQuotesByStatus(Status.Unpaid, limit);
            }

            return _mapper.Map<List<GetQuoteWithServiceDetailsResponse>>(quotes);
        }

        public async Task<GetQuoteWithServiceDetailsResponse> GetQuoteById(Guid id)
        {
            var quotes = await _unitOfWork.Quotes.GetWithServiceDetails(id);
            return _mapper.Map<GetQuoteWithServiceDetailsResponse>(quotes);
        }

        public async Task<GetQuoteResponse> UpdateQuoteStatus(Guid id, Status status)
        {
            var quote = await _unitOfWork.Quotes.Get(id);

            // validate
            if (quote is null)
                throw new KeyNotFoundException("Quote does not exist.");

            if (quote.Status != status)
            {
                quote.Status = status;
                _unitOfWork.Commit();
            }

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
