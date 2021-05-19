using ServiceQuotes.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace ServiceQuotes.Domain.Repositories
{
    public interface IQuoteRepository : IRepository<Quote>
    {
    }
}
