using AutoMapper;
using LinqKit;
using ServiceQuotes.Application.DTOs.Customer;
using ServiceQuotes.Application.DTOs.CustomerAddress;
using ServiceQuotes.Application.Exceptions;
using ServiceQuotes.Application.Filters;
using ServiceQuotes.Application.Interfaces;
using ServiceQuotes.Domain;
using ServiceQuotes.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceQuotes.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CustomerService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<GetCustomerWithImageResponse>> GetAllCustomers(GetCustomersFilter filter)
        {
            // prepare filter predicate
            var predicate = PredicateBuilder.New<Customer>(true);

            if (!string.IsNullOrEmpty(filter?.SearchString))
            {
                predicate = predicate.Or(p => p.CompanyName.ToLower().Contains(filter.SearchString.ToLower()));
                predicate = predicate.Or(p => p.VatNumber.ToLower().Contains(filter.SearchString.ToLower()));
            }

            var customers = await _unitOfWork.Customers.FindWithAccount(predicate);

            return customers.Select(customer => new GetCustomerWithImageResponse()
            {
                Id = customer.Id,
                AccountId = customer.AccountId,
                Image = customer.Account.Image,
                CompanyName = customer.CompanyName,
                VatNumber = customer.VatNumber,
            })
            .ToList();
        }

        public async Task<GetCustomerWithAddressesResponse> GetCustomerById(Guid id)
        {
            var customer = await _unitOfWork.Customers.GetWithAddresses(id);
            return _mapper.Map<GetCustomerWithAddressesResponse>(customer);
        }

        public async Task<GetCustomerResponse> CreateCustomer(CreateCustomerRequest dto)
        {
            // validate
            if (await _unitOfWork.Customers.GetByCompanyName(dto.CompanyName) is not null)
                throw new AppException($"Customer '{dto.CompanyName}' already exists.");

            // map dto to new customer object
            var newCustomer = _mapper.Map<Customer>(dto);

            _unitOfWork.Customers.Add(newCustomer);
            _unitOfWork.Commit();

            return _mapper.Map<GetCustomerResponse>(newCustomer);
        }

        public async Task<GetCustomerResponse> UpdateCustomer(Guid id, UpdateCustomerRequest dto)
        {
            var customer = await _unitOfWork.Customers.Get(id);

            // validate
            if (customer is null) throw new KeyNotFoundException();

            if (!string.IsNullOrEmpty(dto.CompanyName) && customer.CompanyName != dto.CompanyName)
            {
                if (await _unitOfWork.Customers.GetByCompanyName(dto.CompanyName) is not null)
                    throw new AppException($"Customer '{dto.CompanyName}' already exists.");

                customer.CompanyName = dto.CompanyName;
            }

            if (!string.IsNullOrEmpty(dto.VatNumber) && customer.VatNumber != dto.VatNumber)
                customer.VatNumber = dto.VatNumber;

            _unitOfWork.Commit();
            return _mapper.Map<GetCustomerResponse>(customer);
        }

        public async Task<GetCustomerAddressWithCustomerResponse> GetAddressById(Guid customerId, Guid addressId)
        {
            var customerAddress = await _unitOfWork.CustomerAddresses.GetWithCustomerAndAddress(customerId, addressId);
            return _mapper.Map<GetCustomerAddressWithCustomerResponse>(customerAddress);
        }

        public async Task<GetCustomerAddressWithCustomerResponse> CreateAddress(Guid customerId, CreateAddressRequest dto)
        {
            // validate
            if (await _unitOfWork.CustomerAddresses.GetByCustomerIdAndName(customerId, dto.Name) is not null)
                throw new AppException($"Address: '{dto.Name}' already exist.");

            // map dto to new address object
            var address = _mapper.Map<Address>(dto);
            var customerAddress = new CustomerAddress()
            {
                CustomerId = customerId,
                AddressId = address.Id,
                Address = address,
                Name = dto.Name
            };

            _unitOfWork.CustomerAddresses.Add(customerAddress);
            _unitOfWork.Commit();

            return await GetAddressById(customerId, address.Id);
        }

        public async Task DeleteAddress(Guid customerId, Guid addressId)
        {
            var customerAddress = await _unitOfWork.CustomerAddresses.Get(customerId, addressId);
            if (customerAddress is null) throw new KeyNotFoundException();

            _unitOfWork.CustomerAddresses.Remove(customerAddress);
            _unitOfWork.Commit();
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
