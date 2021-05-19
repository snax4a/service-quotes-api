using Microsoft.AspNetCore.Mvc;
using ServiceQuotes.Api.Helpers;
using ServiceQuotes.Application.DTOs.Employee;
using ServiceQuotes.Application.DTOs.Material;
using ServiceQuotes.Application.DTOs.ServiceRequest;
using ServiceQuotes.Application.Filters;
using ServiceQuotes.Application.Interfaces;
using ServiceQuotes.Domain.Entities.Enums;
using ServiceQuotes.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceQuotes.Api.Controllers
{
    public class ServiceRequestsController : BaseController<ServiceRequestsController>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IServiceRequestService _serviceRequestService;

        public ServiceRequestsController(ICustomerRepository customerRepository, IServiceRequestService serviceRequestService)
        {
            _customerRepository = customerRepository;
            _serviceRequestService = serviceRequestService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<GetServiceResponse>>> GetServiceRequests([FromQuery] GetServiceRequestsFilter filter)
        {
            if (Account.Role == Role.Customer)
            {
                return Ok(await _serviceRequestService.GetAllServices(Account, filter));
            }
            else
            {
                return Ok(await _serviceRequestService.GetAllServicesWithCustomerDetails(Account, filter));
            }
        }

        [Authorize]
        [HttpGet("{id:guid}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(GetServiceDetailsResponse), 200)]
        public async Task<ActionResult<GetServiceDetailsResponse>> GetServiceRequestById(Guid id)
        {
            var serviceRequest = await _serviceRequestService.GetServiceRequestById(id);

            if (serviceRequest is null) return NotFound();

            // customer role can get only his own service requests
            if (Account.Role == Role.Customer && serviceRequest.Customer.AccountId != Account.Id)
                return Unauthorized(new { message = "Unauthorized" });

            // employee role can get only service requests he is assigned to
            if (Account.Role == Role.Employee)
            {
                var isAssigned = serviceRequest.AssignedEmployees.Any(e => e.AccountId == Account.Id);
                if (!isAssigned) return Unauthorized(new { message = "Unauthorized" });
            }

            return Ok(serviceRequest);
        }

        [Authorize(Role.Manager, Role.Customer)]
        [HttpPost]
        [ProducesResponseType(404)]
        [ProducesResponseType(201)]
        public async Task<ActionResult<GetServiceResponse>> CreateServiceRequest(Guid id, [FromBody] CreateServiceRequest dto)
        {
            if (Account.Role == Role.Customer)
            {
                var customer = await _customerRepository.GetByAccountId(Account.Id);

                // customer can create only his own service requests
                if (new Guid(dto.CustomerId) != customer.Id)
                    return Unauthorized(new { message = "Unauthorized" });
            }

            var newService = await _serviceRequestService.CreateServiceRequest(dto);
            return CreatedAtAction("GetServiceRequestById", new { id = newService.Id }, newService);
        }

        [Authorize(Role.Manager, Role.Customer)]
        [HttpPut("{id:guid}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        public async Task<ActionResult<GetServiceResponse>> UpdateServiceRequest(Guid id, [FromBody] UpdateServiceRequest dto)
        {
            var serviceRequest = await _serviceRequestService.GetServiceRequestById(id);

            if (serviceRequest is null) return NotFound();

            if (Account.Role == Role.Customer)
            {
                var customer = await _customerRepository.GetByAccountId(Account.Id);

                // customer cant update this value
                if (dto.CustomerId.HasValue && dto.CustomerId != customer.Id)
                    return Unauthorized(new { message = "Unauthorized" });
            }

            await _serviceRequestService.UpdateServiceRequest(id, dto);

            return NoContent();
        }

        [Authorize(Role.Manager, Role.Employee)]
        [HttpPut("{id:guid}/status")]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> UpdateServiceStatus(Guid id, [FromBody] UpdateServiceStatusRequest dto)
        {
            await _serviceRequestService.UpdateServiceStatus(id, dto);
            return NoContent();
        }

        [Authorize(Role.Manager, Role.Employee)]
        [HttpPost("{serviceRequestId:guid}/materials")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<GetEmployeeResponse>> AddMaterial(Guid serviceRequestId, [FromBody] CreateMaterialRequest dto)
        {
            return Ok(await _serviceRequestService.AddMaterial(serviceRequestId, dto));
        }

        [Authorize(Role.Manager, Role.Employee)]
        [HttpDelete("{serviceRequestId:guid}/materials/{materialId:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<ActionResult> RemoveMaterial(Guid serviceRequestId, Guid materialId)
        {
            await _serviceRequestService.RemoveMaterial(materialId);
            return NoContent();
        }
    }
}
