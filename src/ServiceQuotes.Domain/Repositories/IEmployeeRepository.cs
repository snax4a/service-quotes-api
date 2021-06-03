using ServiceQuotes.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ServiceQuotes.Domain.Repositories
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<Employee> GetWithSpecializations(Guid id);
        Task<IEnumerable<Employee>> FindWithSpecializations(Expression<Func<Employee, bool>> predicate);
        Task<Employee> GetByAccountId(Guid accountId);
    }
}
