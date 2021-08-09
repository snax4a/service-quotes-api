using ServiceQuotes.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ServiceQuotes.Domain.Repositories
{
    public interface IPaymentRepository : IRepository<Payment>
    {
        Task<IEnumerable<Payment>> FindWithQuoteAndCustomer(Expression<Func<Payment, bool>> predicate);
        Task<Payment> GetWithQuoteAndCustomer(Guid id);
        Task<Payment> GetWithQuoteAndCustomer(string id);
    }
}
