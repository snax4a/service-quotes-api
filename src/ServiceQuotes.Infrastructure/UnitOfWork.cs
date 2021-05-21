using ServiceQuotes.Domain;
using ServiceQuotes.Domain.Repositories;
using ServiceQuotes.Infrastructure.Context;
using ServiceQuotes.Infrastructure.Repositories;
using System;
using System.Threading.Tasks;

namespace ServiceQuotes.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly Lazy<IAccountRepository> _accountRepository;
        private readonly Lazy<ICustomerRepository> _customerRepository;
        private readonly Lazy<IAddressRepository> _addressRepository;
        private readonly Lazy<ICustomerAddressRepository> _customerAddressRepository;
        private readonly Lazy<IEmployeeRepository> _employeeRepository;
        private readonly Lazy<ISpecializationRepository> _specializationRepository;
        private readonly Lazy<IQuoteRepository> _quoteRepository;
        private readonly Lazy<IPaymentRepository> _paymentRepository;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            _accountRepository = new Lazy<IAccountRepository>(() => new AccountRepository(context));
            _customerRepository = new Lazy<ICustomerRepository>(() => new CustomerRepository(context));
            _addressRepository = new Lazy<IAddressRepository>(() => new AddressRepository(context));
            _customerAddressRepository = new Lazy<ICustomerAddressRepository>(() => new CustomerAddressRepository(context));
            _employeeRepository = new Lazy<IEmployeeRepository>(() => new EmployeeRepository(context));
            _specializationRepository = new Lazy<ISpecializationRepository>(() => new SpecializationRepository(context));
            _quoteRepository = new Lazy<IQuoteRepository>(() => new QuoteRepository(context));
            _paymentRepository = new Lazy<IPaymentRepository>(() => new PaymentRepository(context));
        }

        public IAccountRepository Accounts
        {
            get { return _accountRepository.Value; }
        }

        public ICustomerRepository Customers
        {
            get { return _customerRepository.Value; }
        }

        public IAddressRepository Addresses
        {
            get { return _addressRepository.Value; }
        }

        public ICustomerAddressRepository CustomerAddresses
        {
            get { return _customerAddressRepository.Value; }
        }

        public IEmployeeRepository Employees
        {
            get { return _employeeRepository.Value; }
        }

        public ISpecializationRepository Specializations
        {
            get { return _specializationRepository.Value; }
        }
        
        public IQuoteRepository Quotes
        {
            get { return _quoteRepository.Value; }
        }
        
        public IPaymentRepository Payments
        {
            get { return _paymentRepository.Value; }
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
