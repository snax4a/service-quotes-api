using Microsoft.EntityFrameworkCore;
using ServiceQuotes.Domain.Entities;
using ServiceQuotes.Domain.Repositories;
using ServiceQuotes.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ServiceQuotes.Infrastructure.Repositories
{
    public class ServiceRequestJobValuationRepository : Repository<ServiceRequestJobValuation>, IServiceRequestJobValuationRepository
    {
        public ServiceRequestJobValuationRepository(AppDbContext dbContext) : base(dbContext) { }

        public async Task<ServiceRequestJobValuation> GetByServiceRequestId(Guid serviceRequestId)
        {
            return await _entities
                        .Include(jv => jv.JobValuation)
                        .Include(jv => jv.Employee)
                            .ThenInclude(e => e.Account)
                        .SingleOrDefaultAsync(jv => jv.ServiceRequestId == serviceRequestId);
        }

        public async Task<ServiceRequestJobValuation> GetByEmployeeId(Guid employeeId)
        {
            return await _entities
                        .Include(jv => jv.JobValuation)
                        .Include(jv => jv.Employee)
                            .ThenInclude(e => e.Account)
                        .SingleOrDefaultAsync(jv => jv.EmployeeId == employeeId);
        }

        public async Task<IEnumerable<ServiceRequestJobValuation>> GetLastByEmployeeId(Guid employeeId, int count)
        {
            return await _entities
                        .Include(srjv => srjv.JobValuation)
                        .Where(srjv => srjv.EmployeeId == employeeId)
                        .OrderByDescending(srjv => srjv.Date)
                        .Take(count)
                        .ToListAsync();
        }

        public async Task<ServiceRequestJobValuation> GetByJobValuationId(Guid jobValuationId)
        {
            return await _entities
                        .Include(jv => jv.JobValuation)
                        .Include(jv => jv.Employee)
                            .ThenInclude(e => e.Account)
                        .SingleOrDefaultAsync(jv => jv.JobValuationId == jobValuationId);
        }

        public async Task<IEnumerable<ServiceRequestJobValuation>> GetAllByServiceRequestId(Guid serviceRequestId)
        {
            return await _entities
                        .Include(jv => jv.JobValuation)
                        .Include(jv => jv.Employee)
                            .ThenInclude(e => e.Account)
                        .Where(jv => jv.ServiceRequestId == serviceRequestId)
                        .OrderBy(jv => jv.Date)
                        .ToListAsync();
        }

        public async Task<IEnumerable<ServiceRequestJobValuation>> GetAllByEmployeeId(Guid employeeId)
        {
            return await _entities
                        .Include(jv => jv.JobValuation)
                        .Include(jv => jv.Employee)
                            .ThenInclude(e => e.Account)
                        .Where(jv => jv.EmployeeId == employeeId)
                        .OrderBy(jv => jv.Date)
                        .ToListAsync();
        }
    }
}
