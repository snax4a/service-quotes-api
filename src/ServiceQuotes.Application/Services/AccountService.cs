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
using System;
using System.Collections.Generic;
using System.Linq;
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
            return _mapper.Map<GetAccountResponse>(await _unitOfWork.Accounts.Get(id));
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
            _unitOfWork.Commit();

            return _mapper.Map<GetAccountResponse>(newAccount);
        }

        public async Task<GetAccountResponse> UpdateAccount(Guid id, UpdateAccountRequest dto)
        {
            var account = await _unitOfWork.Accounts.Get(id);
            if (account is null) throw new KeyNotFoundException();

            // hash password if it was entered
            if (!string.IsNullOrEmpty(dto.Password))
                account.PasswordHash = Utilities.HashPassword(dto.Password);

            if (!string.IsNullOrEmpty(dto.Email) && account.Email != dto.Email)
            {
                if (await _unitOfWork.Accounts.GetByEmail(dto.Email) is not null)
                    throw new AppException($"Email '{dto.Email}' is already taken");

                account.Email = dto.Email;
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
            var (refreshToken, account) = await getRefreshToken(token);

            // replace old refresh token with a new one and save
            var newRefreshToken = generateRefreshToken(ipAddress);
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReplacedByToken = newRefreshToken.Token;
            account.RefreshTokens.Add(newRefreshToken);

            _unitOfWork.Commit();

            // generate new jwt
            var jwtToken = Utilities.GenerateJwtToken(account.Id, _appSettings.Secret);

            var response = _mapper.Map<AuthenticatedAccountResponse>(account);
            response.JwtToken = jwtToken;
            response.RefreshToken = newRefreshToken.Token;
            return response;
        }

        public async Task RevokeToken(string token, string ipAddress)
        {
            var (refreshToken, account) = await getRefreshToken(token);

            // revoke token and save
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            await _unitOfWork.CommitAsync();
        }

        private async Task<(RefreshToken, Account)> getRefreshToken(string token)
        {
            var account = await _unitOfWork.Accounts.GetByRefreshToken(token);
            if (account is null) throw new AppException("Invalid token");
            var refreshToken = account.RefreshTokens.Single(x => x.Token == token);
            if (!refreshToken.IsActive) throw new AppException("Invalid token");
            return (refreshToken, account);
        }

        private RefreshToken generateRefreshToken(string ipAddress)
        {
            return new RefreshToken
            {
                Token = randomTokenString(),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
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
