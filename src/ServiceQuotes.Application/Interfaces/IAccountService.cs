using ServiceQuotes.Application.DTOs.Account;
using ServiceQuotes.Application.Filters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceQuotes.Application.Interfaces
{
    public interface IAccountService : IDisposable
    {
        Task<AuthenticatedAccountResponse> Authenticate(AuthenticateRequest model, string ipAddress);

        Task<AuthenticatedAccountResponse> RefreshToken(string token, string ipAddress);

        Task RevokeToken(string token, string ipAddress);

        Task<List<GetAccountResponse>> GetAllAccounts(GetAccountsFilter filter);

        Task<GetAccountResponse> GetAccountById(Guid id);

        Task<GetAccountResponse> CreateAccount(CreateAccountRequest account);

        Task<GetAccountResponse> UpdateAccount(Guid id, UpdateAccountRequest updatedAccount);

        Task DeleteAccount(Guid id);
    }
}
