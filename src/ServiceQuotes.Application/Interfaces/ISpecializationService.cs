using ServiceQuotes.Application.DTOs.Specialization;
using ServiceQuotes.Application.Filters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceQuotes.Application.Interfaces
{
    public interface ISpecializationService : IDisposable
    {
        Task<List<GetSpecializationResponse>> GetAllSpecializations(GetSpecializationsFilter filter);

        Task<GetSpecializationResponse> GetSpecializationById(Guid id);

        Task<GetSpecializationResponse> CreateSpecialization(CreateSpecializationRequest dto);

        Task<GetSpecializationResponse> UpdateSpecialization(Guid id, UpdateSpecializationRequest dto);

        Task RemoveSpecialization(Guid specializationId);
    }
}
