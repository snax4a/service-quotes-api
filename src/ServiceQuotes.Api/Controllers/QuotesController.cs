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

        [Authorize]
        [HttpGet("{id:guid}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(GetQuoteResponse), 200)]
        public async Task<ActionResult<GetQuoteResponse>> GetQuoteById(Guid id)
        {
            var quote = await _quoteService.GetQuoteById(id);

            if (quote is null) return NotFound();

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

            await _quoteService.UpdateQuoteStatus(id, dto);

            return NoContent();
        }
    }
}
