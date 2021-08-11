using ServiceQuotes.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceQuotes.Domain.Repositories
{
    public interface IServiceRequestJobValuationRepository : IRepository<ServiceRequestJobValuation>
    {
        Task<ServiceRequestJobValuation> GetByServiceRequestId(Guid serviceRequestId);
        Task<ServiceRequestJobValuation> GetByEmployeeId(Guid employeeId);
        Task<ServiceRequestJobValuation> GetByJobValuationId(Guid jobValuationId);
        Task<IEnumerable<ServiceRequestJobValuation>> GetAllByServiceRequestId(Guid serviceRequestId);
        Task<IEnumerable<ServiceRequestJobValuation>> GetAllByEmployeeId(Guid employeeId);
        Task<IEnumerable<ServiceRequestJobValuation>> GetLastByEmployeeId(Guid employeeId, int amount);
    }
}
