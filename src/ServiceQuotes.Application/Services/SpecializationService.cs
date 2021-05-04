using AutoMapper;
using LinqKit;
using ServiceQuotes.Application.DTOs.Specialization;
using ServiceQuotes.Application.Exceptions;
using ServiceQuotes.Application.Filters;
using ServiceQuotes.Application.Interfaces;
using ServiceQuotes.Domain;
using ServiceQuotes.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceQuotes.Application.Services
{
    public class SpecializationService : ISpecializationService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public SpecializationService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<GetSpecializationResponse>> GetAllSpecializations(GetSpecializationsFilter filter)
        {
            // prepare filter predicate
            var predicate = PredicateBuilder.New<Specialization>(true);

            if (!string.IsNullOrEmpty(filter?.SearchString))
            {
                predicate = predicate.Or(p => p.Name.ToLower().Contains(filter.SearchString.ToLower()));
            }

            var specializations = await _unitOfWork.Specializations.Find(predicate);

            return _mapper.Map<List<GetSpecializationResponse>>(specializations);
        }

        public async Task<GetSpecializationResponse> GetSpecializationById(Guid id)
        {
            var specialization = await _unitOfWork.Specializations.Get(id);
            return _mapper.Map<GetSpecializationResponse>(specialization);
        }

        public async Task<GetSpecializationResponse> CreateSpecialization(CreateSpecializationRequest dto)
        {
            // validate
            if (await _unitOfWork.Specializations.GetByName(dto.Name) is not null)
                throw new AppException("Specialization already exist.");

            // map dto to new specialization object
            var newSpecialization = _mapper.Map<Specialization>(dto);

            _unitOfWork.Specializations.Add(newSpecialization);
            _unitOfWork.Commit();

            return _mapper.Map<GetSpecializationResponse>(newSpecialization);
        }

        public async Task<GetSpecializationResponse> UpdateSpecialization(Guid id, UpdateSpecializationRequest dto)
        {
            var specialization = await _unitOfWork.Specializations.Get(id);

            // validate
            if (specialization is null) throw new KeyNotFoundException();

            if (!string.IsNullOrEmpty(dto.Name) && specialization.Name != dto.Name)
                specialization.Name = dto.Name;

            _unitOfWork.Commit();

            return _mapper.Map<GetSpecializationResponse>(specialization);
        }

        public async Task DeleteSpecialization(Guid id)
        {
            var specialization = await _unitOfWork.Specializations.Get(id);
            if (specialization is null) throw new KeyNotFoundException();

            _unitOfWork.Specializations.Remove(specialization);
            _unitOfWork.Commit();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _unitOfWork.Dispose();
            }
        }
    }
}
