﻿using Microsoft.AspNetCore.Mvc;
using ServiceQuotes.Api.Helpers;
using ServiceQuotes.Application.DTOs.Payment;
using ServiceQuotes.Application.Filters;
using ServiceQuotes.Application.Interfaces;
using ServiceQuotes.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceQuotes.Api.Controllers
{
    public class PaymentsController : BaseController<PaymentsController>
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [Authorize(Role.Manager)]
        [HttpGet]
        public async Task<ActionResult<List<GetPaymentResponse>>> GetPayments([FromQuery] GetPaymentsFilter filter)
        {
            return Ok(await _paymentService.GetAllPayments(filter));
        }

        [Authorize(Role.Manager, Role.Customer)]
        [HttpGet("{id:guid}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(GetPaymentResponse), 200)]
        public async Task<ActionResult<GetPaymentResponse>> GetPaymentById(Guid id)
        {
            var payment = await _paymentService.GetPaymentById(id);

            if (payment is null) return NotFound();

            return Ok(payment);
        }

        [Authorize(Role.Manager)]
        [HttpPost]
        [ProducesResponseType(404)]
        [ProducesResponseType(201)]
        public async Task<ActionResult<GetPaymentResponse>> CreatePaymentRequest(Guid id, [FromBody] CreatePaymentRequest dto)
        {
            var newPayment = await _paymentService.CreatePayment(dto);
            return CreatedAtAction("GetPaymentById", new { id = newPayment.Id }, newPayment);
        }

        [Authorize(Role.Manager)]
        [HttpPut("{id:guid}/status")]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        public async Task<ActionResult<GetPaymentResponse>> UpdatePaymentStatus(Guid id, [FromBody] UpdatePaymentStatusRequest dto)
        {
            var payment = await _paymentService.GetPaymentById(id);

            if (payment is null) return NotFound();

            await _paymentService.UpdatePaymentStatus(id, dto);

            return NoContent();
        }

        [Authorize(Role.Manager, Role.Customer)]
        [HttpGet("customer/{id:guid}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(GetPaymentResponse), 200)]
        public async Task<ActionResult<List<GetPaymentResponse>>> GetEmployeeByCustomerId(Guid id)
        {
            var payment = await _paymentService.GetPaymentsByCustomerId(id);

            if (payment is null) return NotFound();

            return Ok(payment);
        }

        [Authorize(Role.Manager, Role.Customer)]
        [HttpGet("quote/{id:guid}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(GetPaymentResponse), 200)]
        public async Task<ActionResult<List<GetPaymentResponse>>> GetEmployeeByQuoteId(Guid id)
        {
            var payment = await _paymentService.GetPaymentsByQuoteId(id);

            if (payment is null) return NotFound();

            return Ok(payment);
        }
    }
}
