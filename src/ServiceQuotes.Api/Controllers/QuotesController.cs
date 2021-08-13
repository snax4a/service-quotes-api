using Microsoft.AspNetCore.Mvc;
using ServiceQuotes.Api.Helpers;
using ServiceQuotes.Application.DTOs.Quote;
using ServiceQuotes.Application.Filters;
using ServiceQuotes.Application.Interfaces;
using ServiceQuotes.Domain.Entities.Enums;
using ServiceQuotes.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceQuotes.Api.Controllers
{
    public class QuotesController : BaseController<QuotesController>
    {
        private readonly IQuoteService _quoteService;
        private readonly ICustomerRepository _customerRepository;

        public QuotesController(IQuoteService quoteService, ICustomerRepository customerRepository)
        {
            _quoteService = quoteService;
            _customerRepository = customerRepository;
        }

        [Authorize(Role.Manager, Role.Customer)]
        [HttpGet]
        public async Task<ActionResult<List<GetQuoteResponse>>> GetQuotes([FromQuery] GetQuotesFilter filter)
        {
            if (Account.Role == Role.Customer)
            {
                var customer = await _customerRepository.GetByAccountId(Account.Id);
                filter.CustomerId = customer.Id;
            }

            return Ok(await _quoteService.GetAllQuotes(filter));
        }

        [Authorize(Role.Manager, Role.Customer)]
        [HttpGet("unpaid")]
        public async Task<ActionResult<List<GetQuoteResponse>>> GetTopUnpaidQuotes([FromQuery] GetQuotesFilter filter)
        {
            if (Account.Role == Role.Customer)
            {
                var customer = await _customerRepository.GetByAccountId(Account.Id);
                filter.CustomerId = customer.Id;
            }

            return Ok(await _quoteService.GetTopUnpaidQuotes(filter));
        }

        [Authorize(Role.Manager, Role.Customer)]
        [HttpGet("{id:guid}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(GetQuoteWithServiceDetailsResponse), 200)]
        public async Task<ActionResult<GetQuoteWithServiceDetailsResponse>> GetQuoteById(Guid id)
        {
            var quote = await _quoteService.GetQuoteById(id);

            if (quote is null) return NotFound();

            // customer can get only his own quotes
            if (Account.Role == Role.Customer)
            {
                if (quote.ServiceRequest.Customer.AccountId != Account.Id)
                    return Unauthorized(new { message = "Unauthorized" });
            }

            return Ok(quote);
        }

        [Authorize(Role.Manager)]
        [HttpPut("{id:guid}/status")]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        public async Task<ActionResult<GetQuoteResponse>> UpdateQuoteStatus(Guid id, [FromBody] UpdateQuoteStatusRequest dto)
        {
            var quote = await _quoteService.GetQuoteById(id);

            if (quote is null) return NotFound();

            await _quoteService.UpdateQuoteStatus(id, dto.Status);

            return NoContent();
        }
    }
}
