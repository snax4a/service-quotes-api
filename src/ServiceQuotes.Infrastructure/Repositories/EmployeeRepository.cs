﻿using Microsoft.EntityFrameworkCore;
using ServiceQuotes.Domain.Entities;
using ServiceQuotes.Domain.Repositories;
using ServiceQuotes.Infrastructure.Context;
using System;
using System.Threading.Tasks;

namespace ServiceQuotes.Infrastructure.Repositories
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(AppDbContext dbContext) : base(dbContext) { }

        public async Task<Employee> GetWithSpecializations(Guid id)
        {
            return await _entities
                        .Include(e => e.EmployeeSpecializations)
                        .ThenInclude(es => es.Specialization)
                        .SingleOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Employee> GetByAccountId(Guid accountId)
        {
            return await _entities.SingleOrDefaultAsync(c => c.AccountId == accountId);
        }
    }
}
