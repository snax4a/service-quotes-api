using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceQuotes.Api.Helpers;
using ServiceQuotes.Application.DTOs.Account;
using ServiceQuotes.Application.Exceptions;
using ServiceQuotes.Application.Filters;
using ServiceQuotes.Application.Interfaces;
using ServiceQuotes.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceQuotes.Api.Controllers
{
    public class AccountsController : BaseController<AccountsController>
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public AccountsController(
            IAccountService accountService,
            IMapper mapper,
            ILogger<AccountsController> logger)
        {
            _accountService = accountService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost("authenticate")]
        [ProducesResponseType(typeof(AuthenticatedAccountResponse), 200)]
        public async Task<ActionResult<AuthenticatedAccountResponse>> Authenticate(AuthenticateRequest dto)
        {
            var response = await _accountService.Authenticate(dto, ipAddress());
            setTokenCookie(response.RefreshToken);
            return Ok(response);
        }

        [HttpPost("refresh-token")]
        [ProducesResponseType(typeof(AuthenticatedAccountResponse), 200)]
        public async Task<ActionResult<AuthenticatedAccountResponse>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var response = await _accountService.RefreshToken(refreshToken, ipAddress());
            setTokenCookie(response.RefreshToken);
            return Ok(response);
        }

        [Authorize]
        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken(RevokeTokenRequest model)
        {
            // accept token from request body or cookie
            var token = model.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Token is required" });

            // users can revoke their own tokens and Managers can revoke any tokens
            if (!Account.OwnsToken(token) && Account.Role != Role.Manager)
                return Unauthorized(new { message = "Unauthorized" });

            await _accountService.RevokeToken(token, ipAddress());
            return Ok(new { message = "Token revoked" });
        }

        [Authorize(Role.Manager)]
        [HttpGet]
        public async Task<ActionResult<List<GetAccountResponse>>> GetAccounts([FromQuery] GetAccountsFilter filter)
        {
            return Ok(await _accountService.GetAllAccounts(filter));
        }

        [Authorize]
        [HttpGet("{id:guid}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(GetAccountResponse), 200)]
        public async Task<ActionResult<GetAccountResponse>> GetAccountById(Guid id)
        {
            // users can get their own account and Managers can get any account
            if (id != Account.Id && Account.Role != Role.Manager)
                return Unauthorized(new { message = "Unauthorized" });

            var account = await _accountService.GetAccountById(id);
            if (account == null) return NotFound();
            else return Ok(account);
        }

        [Authorize(Role.Manager)]
        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<ActionResult<GetAccountResponse>> CreateAccount(CreateAccountRequest dto)
        {
            var newAccount = await _accountService.CreateAccount(dto);
            return CreatedAtAction("GetAccountById", new { id = newAccount.Id }, newAccount);
        }

        [Authorize]
        [HttpPut("{id:guid}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        public async Task<ActionResult<GetAccountResponse>> UpdateAccount(Guid id, [FromBody] UpdateAccountRequest dto)
        {
            // users can update their own account and Managers can update any account
            if (id != Account.Id && Account.Role != Role.Manager)
                return Unauthorized(new { message = "Unauthorized" });

            await _accountService.UpdateAccount(id, dto);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteAccount(Guid id)
        {
            // users can delete their own account and Managers can delete any account
            if (id != Account.Id && Account.Role != Role.Manager)
                return Unauthorized(new { message = "Unauthorized" });

            await _accountService.DeleteAccount(id);
            return NoContent();
        }

        // helper methods

        private void setTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7),
                SameSite = SameSiteMode.None,
                Secure = true
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        private string ipAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}
