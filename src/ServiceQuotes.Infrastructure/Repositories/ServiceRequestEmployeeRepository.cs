using ServiceQuotes.Domain.Entities;
using ServiceQuotes.Domain.Repositories;
using ServiceQuotes.Infrastructure.Context;

namespace ServiceQuotes.Infrastructure.Repositories
{
    public class ServiceRequestEmployeeRepository : Repository<ServiceRequestEmployee>, IServiceRequestEmployeeRepository
    {
        public ServiceRequestEmployeeRepository(AppDbContext dbContext) : base(dbContext) { }
    }
}
