using AutoMapper;
using LinqKit;
using ServiceQuotes.Application.DTOs.Customer;
using ServiceQuotes.Application.DTOs.CustomerAddress;
using ServiceQuotes.Application.DTOs.Employee;
using ServiceQuotes.Application.DTOs.Material;
using ServiceQuotes.Application.DTOs.ServiceRequest;
using ServiceQuotes.Application.DTOs.Specialization;
using ServiceQuotes.Application.Exceptions;
using ServiceQuotes.Application.Filters;
using ServiceQuotes.Application.Interfaces;
using ServiceQuotes.Domain;
using ServiceQuotes.Domain.Entities;
using ServiceQuotes.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceQuotes.Application.Services
{
    public class ServiceRequestService : IServiceRequestService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ServiceRequestService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<GetServiceWithAddressResponse>> GetAllServices(Account account, GetServiceRequestsFilter filter)
        {
            // prepare filter predicate
            var predicate = PredicateBuilder.New<ServiceRequest>(true);

            if (!string.IsNullOrEmpty(filter?.SearchString))
            {
                predicate = predicate.Or(p => p.Title.ToLower().Contains(filter.SearchString.ToLower()));
            }

            if (account.Role == Role.Customer)
            {
                // customer can get only his own service requests
                var customer = await _unitOfWork.Customers.GetByAccountId(account.Id);
                predicate = predicate.And(p => p.CustomerId == customer.Id);
            }

            var serviceRequests = await _unitOfWork.ServiceRequests.FindWithAddress(predicate);

            return serviceRequests.Select(sr => new GetServiceWithAddressResponse()
            {
                Id = sr.Id,
                Title = sr.Title,
                Description = sr.Description,
                PlannedExecutionDate = sr.PlannedExecutionDate,
                CompletionDate = sr.CompletionDate,
                Created = sr.Created,
                CustomerId = sr.CustomerId,
                AddressId = sr.AddressId,
                Status = sr.Status.ToString(),
                Address = _mapper.Map<GetAddressResponse>(sr.CustomerAddress.Address)
            })
            .ToList();
        }

        public async Task<List<GetServiceDetailsResponse>> GetAllServicesWithCustomerDetails(
            Account account,
            GetServiceRequestsFilter filter)
        {
            // prepare filter predicate
            var predicate = PredicateBuilder.New<ServiceRequest>(true);

            if (!string.IsNullOrEmpty(filter?.SearchString))
            {
                predicate = predicate.Or(p => p.Title.ToLower().Contains(filter.SearchString.ToLower()));
            }

            if (account.Role == Role.Employee)
            {
                // employee can get only service requests he is assigned to
                var employee = await _unitOfWork.Employees.GetByAccountId(account.Id);
                predicate = predicate.And(p => p.ServiceRequestEmployees.Any(sre => sre.EmployeeId == employee.Id));
            }

            var serviceRequests = await _unitOfWork.ServiceRequests
                                        .FindWithCustomerAndAddressAndEmployees(predicate);

            return serviceRequests.Select(sr => new GetServiceDetailsResponse()
            {
                Id = sr.Id,
                Title = sr.Title,
                Description = sr.Description,
                PlannedExecutionDate = sr.PlannedExecutionDate,
                CompletionDate = sr.CompletionDate,
                Created = sr.Created,
                CustomerId = sr.CustomerId,
                AddressId = sr.AddressId,
                Status = sr.Status.ToString(),
                Customer = new GetCustomerWithImageResponse()
                {
                    Id = sr.CustomerAddress.Customer.Id,
                    AccountId = sr.CustomerAddress.Customer.AccountId,
                    CompanyName = sr.CustomerAddress.Customer.CompanyName,
                    VatNumber = sr.CustomerAddress.Customer.VatNumber,
                    Image = sr.CustomerAddress.Customer.Account.Image
                },
                Address = _mapper.Map<GetAddressResponse>(sr.CustomerAddress.Address)
            })
            .ToList();
        }

        public async Task<GetServiceDetailsResponse> GetServiceRequestById(Guid id)
        {
            var serviceRequest = await _unitOfWork.ServiceRequests.GetWithCustomerAndEmployeesDetails(id);

            if (serviceRequest is null)
                throw new KeyNotFoundException("Service request not found.");

            return new GetServiceDetailsResponse()
            {
                Id = serviceRequest.Id,
                Title = serviceRequest.Title,
                Description = serviceRequest.Description,
                Status = serviceRequest.Status.ToString(),
                PlannedExecutionDate = serviceRequest.PlannedExecutionDate,
                CompletionDate = serviceRequest.CompletionDate,
                Created = serviceRequest.Created,
                CustomerId = serviceRequest.CustomerId,
                AddressId = serviceRequest.AddressId,
                Customer = _mapper.Map<GetCustomerResponse>(serviceRequest.CustomerAddress.Customer),
                Address = _mapper.Map<GetAddressResponse>(serviceRequest.CustomerAddress.Address),
                AssignedEmployees = serviceRequest.ServiceRequestEmployees
                    .Select(sre => new GetEmployeeWithAccountImageResponse()
                    {
                        Id = sre.Employee.Id,
                        AccountId = sre.Employee.AccountId,
                        FirstName = sre.Employee.FirstName,
                        LastName = sre.Employee.LastName,
                        Image = sre.Employee.Account.Image,
                        Specializations = sre.Employee.EmployeeSpecializations
                            .Select(es => new GetSpecializationResponse()
                            {
                                Id = es.Specialization.Id,
                                Name = es.Specialization.Name
                            })
                            .ToList(),
                    })
                    .ToList()
            };
        }

        public async Task<GetServiceResponse> CreateServiceRequest(CreateServiceRequest dto)
        {
            var customer = await _unitOfWork.Customers.GetWithAddresses(new Guid(dto.CustomerId));
            var address = await _unitOfWork.Addresses.Get(new Guid(dto.AddressId));

            // validate
            if (customer is null) throw new KeyNotFoundException("Customer does not exist.");
            if (address is null) throw new KeyNotFoundException("Address does not exist.");
            if (!customer.CustomerAddresses.Any(ca => ca.AddressId == address.Id))
                throw new AppException("Address does not belong to this customer.");

            // map dto to new employee object
            var newService = _mapper.Map<ServiceRequest>(dto);
            newService.Status = Status.New;
            newService.Created = DateTime.UtcNow;

            _unitOfWork.ServiceRequests.Add(newService);
            _unitOfWork.Commit();

            return _mapper.Map<GetServiceResponse>(newService);
        }

        public async Task<GetServiceResponse> UpdateServiceRequest(Guid id, UpdateServiceRequest dto)
        {
            var serviceRequest = await _unitOfWork.ServiceRequests.Get(id);

            // validate
            if (serviceRequest is null)
                throw new KeyNotFoundException("Service does not exist.");

            // update
            if (dto.CustomerId is not null && serviceRequest.CustomerId != dto.CustomerId)
                serviceRequest.CustomerId = dto.CustomerId ?? serviceRequest.CustomerId;

            if (dto.AddressId is not null && serviceRequest.AddressId != dto.AddressId)
                serviceRequest.AddressId = dto.AddressId ?? serviceRequest.AddressId;

            if (!string.IsNullOrEmpty(dto.Title) && serviceRequest.Title != dto.Title)
                serviceRequest.Title = dto.Title;

            if (!string.IsNullOrEmpty(dto.Description) && serviceRequest.Description != dto.Description)
                serviceRequest.Description = dto.Description;

            if (dto.PlannedExecutionDate is not null && serviceRequest.PlannedExecutionDate != dto.PlannedExecutionDate)
                serviceRequest.PlannedExecutionDate = dto.PlannedExecutionDate;

            _unitOfWork.Commit();

            return _mapper.Map<GetServiceResponse>(serviceRequest);
        }

        public async Task<GetServiceResponse> UpdateServiceStatus(Guid id, UpdateServiceStatusRequest dto)
        {
            var serviceRequest = await _unitOfWork.ServiceRequests.Get(id);

            // validate
            if (serviceRequest is null)
                throw new KeyNotFoundException("Service does not exist.");

            switch (dto.Status)
            {
                case Status.Assigned:
                    if (serviceRequest.Status != Status.New)
                        throw new AppException("This status is not allowed for this service state.");
                    break;

                case Status.InProgress:
                    if (serviceRequest.Status != Status.Assigned)
                        throw new AppException("This status is not allowed for this service state.");
                    break;

                case Status.Completed:
                    if (serviceRequest.Status != Status.InProgress)
                        throw new AppException("This status is not allowed for this service state.");
                    break;

                case Status.Cancelled:
                    Status[] possibleStatuses = { Status.New, Status.Assigned };
                    if (!possibleStatuses.Contains(serviceRequest.Status))
                        throw new AppException("This status is not allowed for this service state.");
                    break;

                default:
                    throw new AppException("Incorrect status");
            }

            serviceRequest.Status = dto.Status;

            _unitOfWork.Commit();

            return _mapper.Map<GetServiceResponse>(serviceRequest);
        }

        public async Task<GetMaterialResponse> AddMaterial(Guid serviceRequestId, CreateMaterialRequest dto)
        {
            var serviceRequest = await _unitOfWork.ServiceRequests.Get(serviceRequestId);

            // validate
            if (serviceRequest is null)
                throw new KeyNotFoundException("Service request does not exist.");

            if (serviceRequest.Status != Status.InProgress)
                throw new AppException("This service is not in progress.");

            var newMaterial = _mapper.Map<Material>(dto);
            newMaterial.ServiceRequestId = serviceRequestId;

            _unitOfWork.Materials.Add(newMaterial);
            _unitOfWork.Commit();

            return _mapper.Map<GetMaterialResponse>(newMaterial);
        }

        public async Task RemoveMaterial(Guid materialId)
        {
            var material = await _unitOfWork.Materials.Get(materialId);

            // validate
            if (material is null)
                throw new KeyNotFoundException("Material does not exist.");

            _unitOfWork.Materials.Remove(material);
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
