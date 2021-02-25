using ServiceQuotes.Domain.Entities;
using ServiceQuotes.Domain.Repositories;
using ServiceQuotes.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceQuotes.Infrastructure.Repositories
{
    public class HeroRepository : Repository<Hero>, IHeroRepository
    {
        public HeroRepository(AppDbContext dbContext) : base(dbContext) { }
    }
}
