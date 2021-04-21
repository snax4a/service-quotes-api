using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceQuotes.Api.Helpers;
using ServiceQuotes.Application.DTOs.Customer;
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
        public async Task<ActionResult<List<GetCustomerDTO>>> GetCustomers([FromQuery] GetCustomersFilter filter)
        {
            return Ok(await _customerService.GetAllCustomers(filter));
        }

        [Authorize]
        [HttpGet("{id:guid}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(GetCustomerDTO), 200)]
        public async Task<ActionResult<GetCustomerDTO>> GetCustomerById(Guid id)
        {
            var customer = await _customerService.GetCustomerById(id);

            // users can get their own data and Managers can get any customer
            if (customer.AccountId != Account.Id && Account.Role != Role.Manager)
                return Unauthorized(new { message = "Unauthorized" });

            if (customer == null) return NotFound();
            else return Ok(customer);
        }

        [Authorize]
        [HttpPut("{id:guid}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        public async Task<ActionResult<GetCustomerDTO>> UpdateCustomer(Guid id, [FromBody] UpdateCustomerDTO dto)
        {
            var customer = await _customerService.GetCustomerById(id);

            // users can update their own data and Managers can update any customer
            if (customer.AccountId != Account.Id && Account.Role != Role.Manager)
                return Unauthorized(new { message = "Unauthorized" });

            var updatedCustomer = await _customerService.UpdateCustomer(id, dto);

            if (updatedCustomer == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
