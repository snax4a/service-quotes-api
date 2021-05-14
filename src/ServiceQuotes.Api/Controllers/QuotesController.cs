﻿using Microsoft.AspNetCore.Mvc;
using ServiceQuotes.Api.Helpers;
using ServiceQuotes.Application.DTOs.Quote;
using ServiceQuotes.Application.Filters;
using ServiceQuotes.Application.Interfaces;
using ServiceQuotes.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceQuotes.Api.Controllers
{
    public class QuotesController : BaseController<QuotesController>
    {
        private readonly IQuoteService _quoteService;

        public QuotesController(IQuoteService quoteService)
        {
            _quoteService = quoteService;
        }

        [Authorize(Role.Manager, Role.Customer)]
        [HttpGet]
        public async Task<ActionResult<List<GetQuoteResponse>>> GetQuotes([FromQuery] GetQuotesFilter filter)
        {
            return Ok(await _quoteService.GetAllQuotes(filter));
        }

        [Authorize(Role.Manager)]
        [HttpPut("{id:guid}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        public async Task<ActionResult<GetQuoteResponse>> UpdateQuoteStatus(Guid id, [FromBody] UpdateQuoteStatusRequest dto)
        {
            var employee = await _quoteService.GetQuoteById(id);

            if (employee is null) return NotFound();

            await _quoteService.UpdateQuoteStatus(id, dto);

            return NoContent();
        }
    }
}
