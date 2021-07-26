using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceQuotes.Api.Helpers;
using ServiceQuotes.Application.DTOs.CustomerAddress;
using ServiceQuotes.Application.Filters;
using ServiceQuotes.Application.Interfaces;
using ServiceQuotes.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceQuotes.Api.Controllers
{
    public class CustomerAddressesController : BaseController<CustomerAddressesController>
    {
        private readonly ICustomerAddressService _customerAddressesService;

        public CustomerAddressesController(ICustomerAddressService customerAddressesService)
        {
            _customerAddressesService = customerAddressesService;
        }

        [Authorize(Role.Manager, Role.Customer)]
        [HttpGet("{customerId:guid}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(GetCustomerAddressResponse), 200)]
        public async Task<ActionResult<List<GetCustomerAddressResponse>>> GetCustomerAddresses(Guid customerId, [FromQuery] GetCustomerAddressFilter filter)
        {
            Console.WriteLine("DEBUG");

            var customerAddresses = await _customerAddressesService.GetCustomerAddresses(customerId, filter);

            if (customerAddresses is null) return NotFound();
            return Ok(customerAddresses);
        }
    }
}
