using ServiceQuotes.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace ServiceQuotes.Domain.Repositories
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<Employee> GetWithSpecializations(Guid id);
    }
}
