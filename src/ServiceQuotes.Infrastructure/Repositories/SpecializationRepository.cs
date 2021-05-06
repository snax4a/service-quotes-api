using Microsoft.EntityFrameworkCore;
using ServiceQuotes.Domain.Entities;
using ServiceQuotes.Domain.Repositories;
using ServiceQuotes.Infrastructure.Context;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceQuotes.Infrastructure.Repositories
{
    public class SpecializationRepository : Repository<Specialization>, ISpecializationRepository
    {
        public SpecializationRepository(AppDbContext dbContext) : base(dbContext) { }

        public async Task<Specialization> GetByName(string name)
        {
            return await _entities.Where(s => s.Name == name).SingleOrDefaultAsync();
        }
    }
}
