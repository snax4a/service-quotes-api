using Microsoft.EntityFrameworkCore;
using ServiceQuotes.Domain.Entities;
using ServiceQuotes.Domain.Repositories;
using ServiceQuotes.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceQuotes.Infrastructure.Repositories
{
    public class ServiceRequestRepository : Repository<ServiceRequest>, IServiceRequestRepository
    {
        public ServiceRequestRepository(AppDbContext dbContext) : base(dbContext) { }

        public async Task<ServiceRequest> GetWithCustomerAndEmployeesDetails(Guid id)
        {
            return await _entities
                        .Include(sr => sr.CustomerAddress)
                            .ThenInclude(ca => ca.Address)
                        .Include(sr => sr.CustomerAddress)
                            .ThenInclude(ca => ca.Customer)
                        .SingleOrDefaultAsync(sr => sr.Id == id);
        }

        public async Task<IEnumerable<ServiceRequest>> GetAllWithAddressByCustomertId(Guid customerId)
        {
            return await _entities
                        .Include(sr => sr.CustomerAddress)
                            .ThenInclude(ca => ca.Address)
                        .Where(sr => sr.CustomerId == customerId)
                        .ToListAsync();
        }

        public async Task<IEnumerable<ServiceRequest>> GetAllWithCustomerAndAddressByEmployeeId(Guid employeeId)
        {
            return await _entities
                        .Include(sr => sr.CustomerAddress)
                            .ThenInclude(ca => ca.Address)
                        .Include(sr => sr.CustomerAddress)
                            .ThenInclude(ca => ca.Customer)
                                .ThenInclude(c => c.Account)
                        .Include(sr => sr.ServiceRequestEmployees)
                        .Where(sr => sr.ServiceRequestEmployees.Any(sre => sre.EmployeeId == employeeId))
                        .Select(sr => new ServiceRequest
                        {
                            Id = sr.Id,
                            Title = sr.Title,
                            Description = sr.Description,
                            PlannedExecutionDate = sr.PlannedExecutionDate,
                            CompletionDate = sr.CompletionDate,
                            Created = sr.Created,
                            CustomerId = sr.CustomerId,
                            AddressId = sr.AddressId,
                            Status = sr.Status,
                            CustomerAddress = new CustomerAddress
                            {
                                CustomerId = sr.CustomerId,
                                AddressId = sr.AddressId,
                                Name = sr.CustomerAddress.Name,
                                Address = sr.CustomerAddress.Address,
                                Customer = new Customer
                                {
                                    Id = sr.CustomerId,
                                    AccountId = sr.CustomerAddress.Customer.AccountId,
                                    CompanyName = sr.CustomerAddress.Customer.CompanyName,
                                    VatNumber = sr.CustomerAddress.Customer.VatNumber,
                                    Account = new Account
                                    {
                                        Id = sr.CustomerAddress.Customer.Account.Id,
                                        Image = sr.CustomerAddress.Customer.Account.Image
                                    }
                                }
                            }
                        })
                        .AsNoTracking()
                        .ToListAsync();
        }
    }
}
