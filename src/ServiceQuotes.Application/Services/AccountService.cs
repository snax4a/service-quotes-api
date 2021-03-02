using AutoMapper;
using ServiceQuotes.Application.DTOs.Account;
using ServiceQuotes.Application.Extensions;
using ServiceQuotes.Application.Filters;
using ServiceQuotes.Application.Interfaces;
using ServiceQuotes.Domain.Entities;
using ServiceQuotes.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;
using ServiceQuotes.Application.Exceptions;
using System.Text;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using ServiceQuotes.Application.Helpers;
using Microsoft.Extensions.Options;

namespace ServiceQuotes.Application.Services
{
    public class AccountService : IAccountService
    {

        private readonly IMapper _mapper;
        private readonly IAccountRepository _accountRepository;
        private readonly AppSettings _appSettings;

        public AccountService(IMapper mapper, IAccountRepository accountRepository, IOptions<AppSettings> appSettings)
        {
            _mapper = mapper;
            _accountRepository = accountRepository;
            _appSettings = appSettings.Value;
        }

        public async Task<List<GetAccountDTO>> GetAllAccounts(GetAccountsFilter filter)
        {
            var accounts = _accountRepository
                .GetAll()
                .WhereIf(!string.IsNullOrEmpty(filter?.Email), x => EF.Functions.Like(x.Email, $"%{filter.Email}%"))
                .WhereIf(filter?.Role != null, x => x.Role == filter.Role);
            return await _mapper.ProjectTo<GetAccountDTO>(accounts)
                .ToListAsync();
        }

        public async Task<GetAccountDTO> GetAccountById(Guid id)
        {
            return _mapper.Map<GetAccountDTO>(await _accountRepository.GetById(id));
        }

        public async Task<GetAccountDTO> CreateAccount(CreateAccountDTO dto)
        {
            // validate
            if (await _accountRepository.GetByEmail(dto.Email) != null)
                throw new AppException($"Email '{dto.Email}' is already taken");

            // map dto to new account object
            var newAccount = _mapper.Map<Account>(dto);
            newAccount.PasswordHash = BC.HashPassword(dto.Password);

            var created = _accountRepository.Create(newAccount);
            await _accountRepository.SaveChangesAsync();
            return _mapper.Map<GetAccountDTO>(created);
        }

        public async Task<GetAccountDTO> UpdateAccount(Guid id, UpdateAccountDTO updatedAccount)
        {
            var account = await _accountRepository.GetById(id);
            if (account == null) return null;

            // validate
            if (account.Email != updatedAccount.Email && await _accountRepository.GetByEmail(updatedAccount.Email) != null)
                throw new AppException($"Email '{updatedAccount.Email}' is already taken");

            // hash password if it was entered
            if (!string.IsNullOrEmpty(updatedAccount.Password))
                account.PasswordHash = BC.HashPassword(updatedAccount.Password);

            account.Email = updatedAccount?.Email;
            account.Role = updatedAccount.Role;

            _accountRepository.Update(account);
            await _accountRepository.SaveChangesAsync();
            return _mapper.Map<GetAccountDTO>(account);
        }

        public async Task<bool> DeleteAccount(Guid id)
        {
            await _accountRepository.Delete(id);
            return await _accountRepository.SaveChangesAsync() > 0;
        }

        public async Task<AuthenticatedAccountDTO> Authenticate(AuthenticateDTO model, string ipAddress)
        {
            var account = await _accountRepository.GetByEmail(model.Email);

            if (account == null || !BC.Verify(model.Password, account.PasswordHash))
                throw new AppException("Email or password is incorrect");

            // authentication successful so generate jwt and refresh tokens
            var jwtToken = generateJwtToken(account);
            var refreshToken = generateRefreshToken(ipAddress);

            // save refresh token
            account.RefreshTokens.Add(refreshToken);
            _accountRepository.Update(account);
            await _accountRepository.SaveChangesAsync();

            var response = _mapper.Map<AuthenticatedAccountDTO>(account);
            response.JwtToken = jwtToken;
            response.RefreshToken = refreshToken.Token;
            return response;
        }

        public async Task<AuthenticatedAccountDTO> RefreshToken(string token, string ipAddress)
        {
            var (refreshToken, account) = await getRefreshToken(token);

            // replace old refresh token with a new one and save
            var newRefreshToken = generateRefreshToken(ipAddress);
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReplacedByToken = newRefreshToken.Token;
            account.RefreshTokens.Add(newRefreshToken);
            _accountRepository.Update(account);
            await _accountRepository.SaveChangesAsync();

            // generate new jwt
            var jwtToken = generateJwtToken(account);

            var response = _mapper.Map<AuthenticatedAccountDTO>(account);
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
            _accountRepository.Update(account);
            await _accountRepository.SaveChangesAsync();
        }

        private async Task<(RefreshToken, Account)> getRefreshToken(string token)
        {
            var account = await _accountRepository.GetByRefreshToken(token);
            if (account == null) throw new AppException("Invalid token");
            var refreshToken = account.RefreshTokens.Single(x => x.Token == token);
            if (!refreshToken.IsActive) throw new AppException("Invalid token");
            return (refreshToken, account);
        }

        private string generateJwtToken(Account account)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", account.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
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
                _accountRepository.Dispose();
            }
        }

    }
}
