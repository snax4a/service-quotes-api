using AutoMapper;
using ServiceQuotes.Application.Extensions;
using ServiceQuotes.Application.Filters;
using ServiceQuotes.Application.Interfaces;
using ServiceQuotes.Domain.Entities;
using ServiceQuotes.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ServiceQuotes.Application.Exceptions;
using ServiceQuotes.Application.Helpers;
using Microsoft.Extensions.Options;
using ServiceQuotes.Application.DTOs.Customer;

namespace ServiceQuotes.Application.Services
{
    public class CustomerService : ICustomerService
    {

        private readonly IMapper _mapper;
        private readonly ICustomerRepository _customerRepository;
        private readonly AppSettings _appSettings;

        public CustomerService(IMapper mapper, ICustomerRepository customerRepository, IOptions<AppSettings> appSettings)
        {
            _mapper = mapper;
            _customerRepository = customerRepository;
            _appSettings = appSettings.Value;
        }

        public async Task<List<GetCustomerDTO>> GetAllCustomers(GetCustomersFilter filter)
        {
            var customers = _customerRepository
                .GetAll()
                .WhereIf(!string.IsNullOrEmpty(filter?.CompanyName), x => EF.Functions.Like(x.CompanyName, $"%{filter.CompanyName}%"))
                .WhereIf(!string.IsNullOrEmpty(filter?.VatNumber), x => EF.Functions.Like(x.VatNumber, $"%{filter.VatNumber}%"));
            return await _mapper.ProjectTo<GetCustomerDTO>(customers).ToListAsync();
        }

        public async Task<GetCustomerDTO> GetCustomerById(Guid id)
        {
            return _mapper.Map<GetCustomerDTO>(await _customerRepository.GetById(id));
        }

        public async Task<GetCustomerDTO> CreateCustomer(CreateCustomerDTO dto)
        {
            // validate
            if (await _customerRepository.GetByCompanyName(dto.CompanyName) != null)
                throw new AppException($"Customer '{dto.CompanyName}' already exists.");

            // map dto to new customer object
            var newCustomer = _mapper.Map<Customer>(dto);

            var created = _customerRepository.Create(newCustomer);
            await _customerRepository.SaveChangesAsync();
            return _mapper.Map<GetCustomerDTO>(created);
        }

        public async Task<GetCustomerDTO> UpdateCustomer(Guid id, UpdateCustomerDTO updatedCustomer)
        {
            var customer = await _customerRepository.GetById(id);
            if (customer == null) return null;

            // validate
            var doesCompanyExist = await _customerRepository.GetByCompanyName(updatedCustomer.CompanyName) != null;
            if (customer.CompanyName != updatedCustomer.CompanyName && doesCompanyExist)
                throw new AppException($"Customer '{updatedCustomer.CompanyName}' already exists.");

            customer.CompanyName = updatedCustomer?.CompanyName;
            customer.VatNumber = updatedCustomer?.VatNumber;

            _customerRepository.Update(customer);
            await _customerRepository.SaveChangesAsync();
            return _mapper.Map<GetCustomerDTO>(customer);
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
                _customerRepository.Dispose();
            }
        }
    }
}
