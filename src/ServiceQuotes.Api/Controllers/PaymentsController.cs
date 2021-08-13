using Microsoft.AspNetCore.Mvc;
using ServiceQuotes.Api.Helpers;
using ServiceQuotes.Application.DTOs.Payment;
using ServiceQuotes.Application.DTOs.Paynow;
using ServiceQuotes.Application.Filters;
using ServiceQuotes.Application.Interfaces;
using ServiceQuotes.Domain.Entities.Enums;
using ServiceQuotes.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceQuotes.Api.Controllers
{
    public class PaymentsController : BaseController<PaymentsController>
    {
        private readonly IPaymentService _paymentService;
        private readonly ICustomerRepository _customerRepository;

        public PaymentsController(IPaymentService paymentService, ICustomerRepository customerRepository)
        {
            _paymentService = paymentService;
            _customerRepository = customerRepository;
        }

        [Authorize(Role.Manager, Role.Customer)]
        [HttpGet]
        public async Task<ActionResult<List<GetPaymentResponse>>> GetPayments([FromQuery] GetPaymentsFilter filter)
        {
            return Ok(await _paymentService.GetAllPayments(Account, filter));
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

            // customer can get only his own payments
            if (Account.Role == Role.Customer)
            {
                var customer = await _customerRepository.GetByAccountId(Account.Id);
                if (payment.CustomerId != customer.Id)
                    return Unauthorized(new { message = "Unauthorized" });
            }

            return Ok(payment);
        }

        [Authorize(Role.Manager, Role.Customer)]
        [HttpGet("transaction/{transactionId}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(GetPaymentResponse), 200)]
        public async Task<ActionResult<GetPaymentResponse>> GetPaymentByTransactionId(string transactionId)
        {
            var payment = await _paymentService.GetPaymentByTransactionId(transactionId);

            if (payment is null) return NotFound();

            // customer can get only his own payments
            if (Account.Role == Role.Customer)
            {
                var customer = await _customerRepository.GetByAccountId(Account.Id);
                if (payment.CustomerId != customer.Id)
                    return Unauthorized(new { message = "Unauthorized" });
            }

            return Ok(payment);
        }

        [Authorize(Role.Manager, Role.Customer)]
        [HttpGet("quote/{id:guid}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(GetPaymentResponse), 200)]
        public async Task<ActionResult<List<GetPaymentResponse>>> GetPaymentsByQuoteId(Guid id)
        {
            var payment = await _paymentService.GetPaymentsByQuoteId(id);

            if (payment is null) return NotFound();

            return Ok(payment);
        }

        [Authorize(Role.Customer)]
        [HttpPost]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(201)]
        public async Task<ActionResult<GetPaymentResponse>> CreatePayment([FromBody] CreatePaymentRequest dto)
        {
            var response = await _paymentService.CreatePaymentForQuote(dto, Account.Id);
            return CreatedAtAction("GetPaymentById", new { id = response.Payment.Id }, response);
        }

        [HttpPost("paynow/notification")]
        [ProducesResponseType(202)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> PaynowNotification([FromBody] PaynowNotificationRequest dto)
        {
            if (Request.Headers.ContainsKey("Signature"))
            {
                var signatureHeader = Request.Headers["Signature"];
                await _paymentService.ProcessPaynowNotification(dto, signatureHeader);
                return Accepted();
            }

            return BadRequest();
        }
    }
}
