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
        IEmployeeRepository Employees { get; }
        ISpecializationRepository Specializations { get; }
        IQuoteRepository Quotes { get; }
        IServiceRequestRepository ServiceRequests { get; }
        IMaterialRepository Materials { get; }
        IPaymentRepository Payments { get; }

        void Commit();
        Task CommitAsync();
    }
}
