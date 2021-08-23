using AutoMapper;
using LinqKit;
using Microsoft.Extensions.Options;
using ServiceQuotes.Application.DTOs.Account;
using ServiceQuotes.Application.Exceptions;
using ServiceQuotes.Application.Filters;
using ServiceQuotes.Application.Helpers;
using ServiceQuotes.Application.Interfaces;
using ServiceQuotes.Domain;
using ServiceQuotes.Domain.Entities;
using ServiceQuotes.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace ServiceQuotes.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppSettings _appSettings;

        public AccountService(IMapper mapper, IUnitOfWork unitOfWork, IOptions<AppSettings> appSettings)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _appSettings = appSettings.Value;
        }

        public async Task<List<GetAccountResponse>> GetAllAccounts(GetAccountsFilter filter)
        {
            // prepare filter predicate
            var predicate = PredicateBuilder.New<Account>(true);

            if (!string.IsNullOrEmpty(filter?.SearchString))
            {
                predicate = predicate.Or(p => p.Email.ToLower().Contains(filter.SearchString.ToLower()));
                predicate = predicate.Or(p => p.Id.ToString().ToLower().Contains(filter.SearchString.ToLower()));
            }

            if (filter?.Role is not null)
            {
                predicate = predicate.And(p => p.Role == filter.Role);
            }

            var accounts = await _unitOfWork.Accounts.Find(predicate);

            return _mapper.Map<List<GetAccountResponse>>(accounts);
        }

        public async Task<GetAccountResponse> GetAccountById(Guid id)
        {
            var account = await _unitOfWork.Accounts.Get(id);

            if (account is null)
                throw new KeyNotFoundException("Accound does not exist.");

            if (account.Role == Role.Customer)
            {
                var customer = await _unitOfWork.Customers.GetByAccountId(account.Id);

                if (customer is null)
                    throw new KeyNotFoundException("Could not find customer record related to this account.");

                return new GetAccountWithCustomerResponse()
                {
                    Id = account.Id,
                    Email = account.Email,
                    Role = account.Role.ToString(),
                    Image = account.Image,
                    Created = account.Created,
                    Updated = account.Updated,
                    CustomerId = customer.Id,
                    CompanyName = customer.CompanyName,
                    VatNumber = customer.VatNumber
                };
            }
            else
            {
                var employee = await _unitOfWork.Employees.GetByAccountId(account.Id);

                if (employee is null)
                    throw new KeyNotFoundException("Could not find employee record related to this account.");

                return new GetAccountWithEmployeeResponse()
                {
                    Id = account.Id,
                    Email = account.Email,
                    Role = account.Role.ToString(),
                    Image = account.Image,
                    Created = account.Created,
                    Updated = account.Updated,
                    EmployeeId = employee.Id,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName
                };
            }
        }

        public async Task<GetAccountResponse> CreateAccount(CreateAccountRequest dto)
        {
            // validate
            if (await _unitOfWork.Accounts.GetByEmail(dto.Email) != null)
                throw new AppException($"Email '{dto.Email}' is already taken");

            // map dto to new account object
            var newAccount = _mapper.Map<Account>(dto);
            newAccount.PasswordHash = Utilities.HashPassword(dto.Password);
            newAccount.Created = DateTime.Now;

            _unitOfWork.Accounts.Add(newAccount);

            // For customer role we create customer entity
            // For employee and manager role we create employee entity
            if (dto.Role == Role.Customer)
            {
                if (string.IsNullOrEmpty(dto.CompanyName))
                    throw new AppException("Company name is required");

                if (string.IsNullOrEmpty(dto.VatNumber))
                    throw new AppException("Vat number is required");

                _unitOfWork.Customers.Add(new Customer()
                {
                    AccountId = newAccount.Id,
                    CompanyName = dto.CompanyName,
                    VatNumber = dto.VatNumber
                });
            }
            else
            {
                if (string.IsNullOrEmpty(dto.FirstName))
                    throw new AppException("First name is required");

                if (string.IsNullOrEmpty(dto.LastName))
                    throw new AppException("Last name is required");

                _unitOfWork.Employees.Add(new Employee()
                {
                    AccountId = newAccount.Id,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName
                });
            }

            _unitOfWork.Commit();

            return await GetAccountById(newAccount.Id);
        }

        public async Task<GetAccountResponse> UpdateAccount(Guid id, UpdateAccountRequest dto)
        {
            var account = await _unitOfWork.Accounts.Get(id);
            if (account is null)
                throw new KeyNotFoundException("Account does not exist.");

            if (!string.IsNullOrEmpty(dto.Password))
            {
                // validate
                if (dto.Password != dto.RepeatPassword)
                    throw new AppException("Passwords do not match.");

                if (dto.Password.Length < 6)
                    throw new AppException("Password must be at least 6 characters long.");

                // hash password
                account.PasswordHash = Utilities.HashPassword(dto.Password);
            }

            if (!string.IsNullOrEmpty(dto.Email) && account.Email != dto.Email)
            {
                if (await _unitOfWork.Accounts.GetByEmail(dto.Email) is not null)
                    throw new AppException($"Email '{dto.Email}' is already taken");

                account.Email = dto.Email;
            }

            if (dto.Image?.Length > 0)
            {
                int ALLOWED_FILE_SIZE = 3 * 1024 * 1024; // ~= 3 MB
                if (dto.Image.Length > ALLOWED_FILE_SIZE)
                    throw new AppException("Image size is too big.");

                var isValidImage = Utilities.IsValidImage(dto.Image);

                if (!isValidImage)
                    throw new AppException("Uploaded image is invalid.");

                account.Image = dto.Image;
            }

            // update role specific fields
            if (account.Role == Role.Customer)
            {
                var customer = await _unitOfWork.Customers.GetByAccountId(account.Id);

                if (!string.IsNullOrEmpty(dto.CompanyName) && customer.CompanyName != dto.CompanyName)
                    customer.CompanyName = dto.CompanyName;

                if (!string.IsNullOrEmpty(dto.VatNumber) && customer.VatNumber != dto.VatNumber)
                    customer.VatNumber = dto.VatNumber;
            }
            else
            {
                var employee = await _unitOfWork.Employees.GetByAccountId(account.Id);

                if (!string.IsNullOrEmpty(dto.FirstName) && employee.FirstName != dto.FirstName)
                    employee.FirstName = dto.FirstName;

                if (!string.IsNullOrEmpty(dto.LastName) && employee.LastName != dto.LastName)
                    employee.LastName = dto.LastName;
            }

            account.Updated = DateTime.Now;
            _unitOfWork.Commit();

            return _mapper.Map<GetAccountResponse>(account);
        }

        public async Task DeleteAccount(Guid id)
        {
            var account = await _unitOfWork.Accounts.Get(id);
            if (account is null) throw new KeyNotFoundException();

            _unitOfWork.Accounts.Remove(account);
            _unitOfWork.Commit();
        }

        public async Task<AuthenticatedAccountResponse> Authenticate(AuthenticateRequest dto, string ipAddress)
        {
            var account = await _unitOfWork.Accounts.GetByEmail(dto.Email);

            if (account is null || !Utilities.VerifyPassword(dto.Password, account.PasswordHash))
                throw new AppException("Email or password is incorrect");

            // authentication successful so generate jwt and refresh tokens
            var jwtToken = Utilities.GenerateJwtToken(account.Id, _appSettings.Secret);
            var refreshToken = generateRefreshToken(ipAddress);

            // save refresh token
            account.RefreshTokens.Add(refreshToken);
            _unitOfWork.Commit();

            var response = _mapper.Map<AuthenticatedAccountResponse>(account);
            response.JwtToken = jwtToken;
            response.RefreshToken = refreshToken.Token;
            return response;
        }

        public async Task<AuthenticatedAccountResponse> RefreshToken(string token, string ipAddress)
        {
            var refreshToken = await getRefreshToken(token);
            var account = await _unitOfWork.Accounts.Get(refreshToken.AccountId);

            // replace old refresh token with a new one and save
            var newRefreshToken = generateRefreshToken(ipAddress);
            account.RefreshTokens.Add(newRefreshToken);

            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = IPAddress.Parse(ipAddress);
            refreshToken.ReplacedByToken = newRefreshToken.Token;

            _unitOfWork.RefreshTokens.Add(newRefreshToken);
            _unitOfWork.Commit();

            // generate new jwt
            var jwtToken = Utilities.GenerateJwtToken(account.Id, _appSettings.Secret);

            var response = _mapper.Map<AuthenticatedAccountResponse>(account);
            response.JwtToken = jwtToken;
            response.RefreshToken = newRefreshToken.Token;
            return response;
        }

        public async Task<bool> DoesAccountOwnToken(Guid accountId, string token)
        {
            var account = await _unitOfWork.RefreshTokens.GetByToken(token);
            return accountId == account.Id;
        }

        public async Task RevokeToken(string token, string ipAddress)
        {
            var refreshToken = await getRefreshToken(token);

            // revoke token and save
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = IPAddress.Parse(ipAddress);
            await _unitOfWork.CommitAsync();
        }

        private async Task<RefreshToken> getRefreshToken(string token)
        {
            var refreshToken = await _unitOfWork.RefreshTokens.GetByToken(token);
            if (refreshToken is null || !refreshToken.IsActive)
                throw new AppException("Invalid token");

            return refreshToken;
        }

        private RefreshToken generateRefreshToken(string ipAddress)
        {
            return new RefreshToken
            {
                Token = randomTokenString(),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                CreatedByIp = IPAddress.Parse(ipAddress)
            };
        }

        private string randomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            // convert random bytes to hex string
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _unitOfWork.Dispose();
            }
        }

    }
}
