using ServiceQuotes.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ServiceQuotes.Domain.Repositories
{
    public interface IServiceRequestRepository : IRepository<ServiceRequest>
    {
        Task<ServiceRequest> GetWithCustomerAndEmployeesDetails(Guid id);
        Task<IEnumerable<ServiceRequest>> FindWithAddress(Expression<Func<ServiceRequest, bool>> predicate);
        Task<IEnumerable<ServiceRequest>> FindWithCustomerAndAddressAndEmployees(
            Expression<Func<ServiceRequest, bool>> predicate
        );
    }
}
