using AutoMapper;
using LinqKit;
using ServiceQuotes.Application.DTOs.CustomerAddress;
using ServiceQuotes.Application.Filters;
using ServiceQuotes.Application.Interfaces;
using ServiceQuotes.Domain;
using ServiceQuotes.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceQuotes.Application.Services
{
    public class CustomerAddressService : ICustomerAddressService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CustomerAddressService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<GetCustomerAddressResponse>> GetCustomerAddresses(Guid customerId, GetCustomerAddressFilter filter)
        {
            // prepare filter predicate
            var predicate = PredicateBuilder.New<CustomerAddress>(true);

            if (!string.IsNullOrEmpty(filter?.SearchString))
            {
                predicate = predicate.Or(p => p.Name.ToLower().Contains(filter.SearchString.ToLower()));
                predicate = predicate.Or(p => p.Address.Street.ToLower().Contains(filter.SearchString.ToLower()));
                predicate = predicate.Or(p => p.Address.City.ToLower().Contains(filter.SearchString.ToLower()));
                predicate = predicate.Or(p => p.Address.ZipCode.ToLower().Contains(filter.SearchString.ToLower()));
                predicate = predicate.Or(p => p.Address.PhoneNumber.ToLower().Contains(filter.SearchString.ToLower()));
            }

            if (!string.IsNullOrEmpty(filter?.City))
            {
                predicate = predicate.And(p => p.Address.City.ToLower().Contains(filter.City.ToLower()));
            }

            var customerAddresses = await _unitOfWork.CustomerAddresses.FindWithAddress(customerId, predicate);
            return _mapper.Map<List<GetCustomerAddressResponse>>(customerAddresses);
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
