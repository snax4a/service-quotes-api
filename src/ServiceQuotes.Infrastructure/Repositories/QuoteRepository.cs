using Microsoft.EntityFrameworkCore;
using ServiceQuotes.Domain.Entities;
using ServiceQuotes.Domain.Repositories;
using ServiceQuotes.Infrastructure.Context;
using System;
using System.Threading.Tasks;

namespace ServiceQuotes.Infrastructure.Repositories
{
    public class QuoteRepository : Repository<Quote>, IQuoteRepository
    {
        public QuoteRepository(AppDbContext dbContext) : base(dbContext) { }

    }
}