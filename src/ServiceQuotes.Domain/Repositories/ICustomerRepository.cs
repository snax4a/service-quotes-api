using ServiceQuotes.Domain.Core.Interfaces;
using ServiceQuotes.Domain.Entities;
using System.Threading.Tasks;

namespace ServiceQuotes.Domain.Repositories
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<Customer> GetByCompanyName(string companyName);
    }
}
