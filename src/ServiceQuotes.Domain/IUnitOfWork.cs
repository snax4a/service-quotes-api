using ServiceQuotes.Domain.Repositories;
using System;
using System.Threading.Tasks;

namespace ServiceQuotes.Domain
{
    public interface IUnitOfWork : IDisposable
    {
        IAccountRepository Accounts { get; }
        ICustomerRepository Customers { get; }
        IAddressRepository Addresses { get; }
        ICustomerAddressRepository CustomerAddresses { get; }

        void Commit();
        Task CommitAsync();
    }
}
