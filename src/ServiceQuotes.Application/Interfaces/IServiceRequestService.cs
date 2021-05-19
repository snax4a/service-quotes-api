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

        Task<GetMaterialResponse> AddMaterial(Guid serviceRequestId, CreateMaterialRequest dto);

        Task RemoveMaterial(Guid materialId);
    }
}
