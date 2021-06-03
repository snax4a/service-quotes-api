using ServiceQuotes.Domain.Entities;
using ServiceQuotes.Domain.Repositories;
using ServiceQuotes.Infrastructure.Context;

namespace ServiceQuotes.Infrastructure.Repositories
{
    public class JobValuationRepository : Repository<JobValuation>, IJobValuationRepository
    {
        public JobValuationRepository(AppDbContext dbContext) : base(dbContext) { }
    }
}
