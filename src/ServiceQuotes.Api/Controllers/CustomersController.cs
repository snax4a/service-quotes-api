using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceQuotes.Api.Helpers;
using ServiceQuotes.Application.DTOs.Customer;
using ServiceQuotes.Application.DTOs.CustomerAddress;
using ServiceQuotes.Application.Filters;
using ServiceQuotes.Application.Interfaces;
using ServiceQuotes.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceQuotes.Api.Controllers
{
    public class CustomersController : BaseController<CustomersController>
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger _logger;

        public CustomersController(
            ICustomerService customerService,
            ILogger<CustomersController> logger)
        {
            _customerService = customerService;
            _logger = logger;
        }

        [Authorize(Role.Manager)]
        [HttpGet]
        public async Task<ActionResult<List<GetCustomerResponse>>> GetCustomers([FromQuery] GetCustomersFilter filter)
        {
            return Ok(await _customerService.GetAllCustomers(filter));
        }

        [Authorize(Role.Manager, Role.Customer)]
        [HttpGet("{id:guid}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(GetCustomerResponse), 200)]
        public async Task<ActionResult<GetCustomerResponse>> GetCustomerById(Guid id)
        {
            var customer = await _customerService.GetCustomerById(id);

            if (customer is null) return NotFound();

            // users can get their own data and Managers can get any customer
            if (customer.AccountId != Account.Id && Account.Role != Role.Manager)
                return Unauthorized(new { message = "Unauthorized" });

            return Ok(customer);
        }

        [Authorize(Role.Manager, Role.Customer)]
        [HttpPut("{id:guid}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        public async Task<ActionResult<GetCustomerResponse>> UpdateCustomer(Guid id, [FromBody] UpdateCustomerRequest dto)
        {
            var customer = await _customerService.GetCustomerById(id);

            if (customer is null) return NotFound();

            // users can update their own data and Managers can update any customer
            if (customer.AccountId != Account.Id && Account.Role != Role.Manager)
                return Unauthorized(new { message = "Unauthorized" });

            await _customerService.UpdateCustomer(id, dto);

            return NoContent();
        }

        [Authorize(Role.Manager, Role.Customer)]
        [HttpGet("{customerId:guid}/address/{addressId:guid}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(GetAddressResponse), 200)]
        public async Task<ActionResult<GetAddressResponse>> GetAddressById(Guid customerId, Guid addressId)
        {
            var customerAddress = await _customerService.GetAddressById(customerId, addressId);

            if (customerAddress is null) return NotFound();

            // users can get their own data and Managers can get any customer
            if (customerAddress.Customer.AccountId != Account.Id && Account.Role != Role.Manager)
                return Unauthorized(new { message = "Unauthorized" });

            return Ok(customerAddress);
        }

        [Authorize(Role.Manager, Role.Customer)]
        [HttpPost("{customerId:guid}/address")]
        [ProducesResponseType(201)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<GetCustomerAddressResponse>> CreateAddress(Guid customerId, [FromBody] CreateAddressRequest dto)
        {
            var customer = await _customerService.GetCustomerById(customerId);

            // validate
            if (customer is null)
                return NotFound(new { message = "Customer does not exist" });
            if (customer.AccountId != Account.Id && Account.Role != Role.Manager)
                return Unauthorized(new { message = "Unauthorized" });

            var customerAddress = await _customerService.CreateAddress(customerId, dto);
            return CreatedAtAction("GetAddressById", new
            {
                customerId = customerAddress.Customer.Id,
                addressId = customerAddress.Address.Id
            }, customerAddress);
        }

        [Authorize(Role.Manager)]
        [HttpDelete("{customerId:guid}/address/{addressId:guid}")]
        [ProducesResponseType(201)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteAddress(Guid customerId, Guid addressId)
        {
            await _customerService.DeleteAddress(customerId, addressId);
            return NoContent();
        }

        [Authorize(Role.Manager, Role.Customer)]
        [HttpPut("{customerId:guid}/address/{addressId:guid}")]
        [ProducesResponseType(201)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<GetAddressResponse>> UpdateAddress(Guid customerId, Guid addressId, [FromBody] UpdateAddressRequest dto)
        {
            var customerAddress = await _customerService.GetAddressById(customerId, addressId);

            // validate
            if (customerAddress is null)
                return NotFound(new { message = "Customer's address does not exist" });
            if (customerAddress.Customer.AccountId != Account.Id && Account.Role != Role.Manager)
                return Unauthorized(new { message = "Unauthorized" });

            await _customerService.UpdateAddress(customerId, addressId, dto);

            return NoContent();
        }
    }
}
