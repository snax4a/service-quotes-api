using ServiceQuotes.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace ServiceQuotes.Domain.Repositories
{
    public interface ISpecializationRepository : IRepository<Specialization>
    {
        Task<Specialization> GetByName(string name);
    }
}
