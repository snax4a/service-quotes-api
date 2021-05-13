using ServiceQuotes.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceQuotes.Domain.Repositories
{
    public interface IServiceRequestRepository : IRepository<ServiceRequest>
    {
        Task<ServiceRequest> GetWithCustomerAndEmployeesDetails(Guid id);
        Task<IEnumerable<ServiceRequest>> GetAllWithAddressByCustomertId(Guid customerId);
        Task<IEnumerable<ServiceRequest>> GetAllWithCustomerAndAddressByEmployeeId(Guid employeeId);
    }
}
