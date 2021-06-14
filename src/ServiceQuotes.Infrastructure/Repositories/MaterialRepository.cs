using Microsoft.EntityFrameworkCore;
using ServiceQuotes.Domain.Entities;
using ServiceQuotes.Domain.Repositories;
using ServiceQuotes.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceQuotes.Infrastructure.Repositories
{
    public class MaterialRepository : Repository<Material>, IMaterialRepository
    {
        public MaterialRepository(AppDbContext dbContext) : base(dbContext) { }

        public async Task<IEnumerable<Material>> GetAllByServiceRequestId(Guid serviceRequestId)
        {
            return await _entities
                        .Where(m => m.ServiceRequestId == serviceRequestId)
                        .OrderBy(m => m.Description)
                        .ToListAsync();
        }
    }
}
