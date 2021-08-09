using ServiceQuotes.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ServiceQuotes.Domain.Repositories
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<Customer> GetByCompanyName(string companyName);
        Task<Customer> GetWithAddresses(Guid id);
        Task<Customer> GetByAccountId(Guid accountId);
        Task<IEnumerable<Customer>> FindWithAccount(Expression<Func<Customer, bool>> predicate);
    }
}
