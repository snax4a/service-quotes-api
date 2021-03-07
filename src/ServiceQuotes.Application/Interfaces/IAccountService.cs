using ServiceQuotes.Application.DTOs.Account;
using ServiceQuotes.Application.Filters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceQuotes.Application.Interfaces
{
    public interface IAccountService : IDisposable
    {
        Task<AuthenticatedAccountDTO> Authenticate(AuthenticateDTO model, string ipAddress);

        Task<AuthenticatedAccountDTO> RefreshToken(string token, string ipAddress);

        Task RevokeToken(string token, string ipAddress);

        Task<List<GetAccountDTO>> GetAllAccounts(GetAccountsFilter filter);

        Task<GetAccountDTO> GetAccountById(Guid id);

        Task<GetAccountDTO> CreateAccount(CreateAccountDTO account);

        Task<GetAccountDTO> UpdateAccount(Guid id, UpdateAccountDTO updatedAccount);

        Task<bool> DeleteAccount(Guid id);
    }
}
