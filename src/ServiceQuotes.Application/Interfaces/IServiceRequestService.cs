using ServiceQuotes.Application.DTOs.JobValuation;
using ServiceQuotes.Application.DTOs.Material;
using ServiceQuotes.Application.DTOs.ServiceRequest;
using ServiceQuotes.Application.Filters;
using ServiceQuotes.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceQuotes.Application.Interfaces
{
    public interface IServiceRequestService : IDisposable
    {
        Task<List<GetServiceWithAddressResponse>> GetAllServices(Account account, GetServiceRequestsFilter filter);

        Task<List<GetServiceDetailsResponse>> GetAllServicesWithCustomerDetails(Account account, GetServiceRequestsFilter filter);

        Task<GetServiceDetailsResponse> GetServiceRequestById(Guid id);

        Task<GetServiceResponse> CreateServiceRequest(CreateServiceRequest dto);

        Task<GetServiceResponse> UpdateServiceRequest(Guid id, UpdateServiceRequest dto);

        Task<GetServiceResponse> UpdateServiceStatus(Guid id, UpdateServiceStatusRequest dto);

        Task<List<GetMaterialResponse>> GetMaterials(Guid serviceRequestId);

        Task<GetMaterialResponse> AddMaterial(Guid serviceRequestId, CreateMaterialRequest dto);

        Task RemoveMaterial(Guid materialId);

        Task<List<GetServiceRequestJobValuationResponse>> GetJobValuations(Guid serviceRequestId);

        Task<GetJobValuationResponse> AddJobValuation(Guid serviceRequestId, CreateJobValuationRequest dto);

        Task RemoveJobValuation(Guid jobValuationId);

        Task AssignEmployee(Guid serviceRequestId, AssignEmployeeRequest dto);

        Task RemoveEmployee(Guid serviceRequestId, AssignEmployeeRequest dto);
    }
}
