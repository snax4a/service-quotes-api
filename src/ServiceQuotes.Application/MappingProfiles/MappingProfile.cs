using AutoMapper;
using ServiceQuotes.Application.DTOs.Account;
using ServiceQuotes.Application.DTOs.Customer;
using ServiceQuotes.Application.DTOs.CustomerAddress;
using ServiceQuotes.Application.DTOs.Employee;
using ServiceQuotes.Application.DTOs.JobValuation;
using ServiceQuotes.Application.DTOs.Material;
using ServiceQuotes.Application.DTOs.Payment;
using ServiceQuotes.Application.DTOs.Quote;
using ServiceQuotes.Application.DTOs.ServiceRequest;
using ServiceQuotes.Application.DTOs.Specialization;
using ServiceQuotes.Domain.Entities;

namespace ServiceQuotes.Application.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            /// Account Map
            CreateMap<Account, GetAccountResponse>().ReverseMap();
            CreateMap<CreateAccountRequest, Account>();
            CreateMap<UpdateAccountRequest, Account>();
            CreateMap<Account, AuthenticatedAccountResponse>();
            ///

            /// Customer Map
            CreateMap<Customer, GetCustomerResponse>().ReverseMap();
            CreateMap<Customer, GetCustomerWithAddressesResponse>();
            CreateMap<Customer, GetCustomerWithImageResponse>();
            CreateMap<CreateCustomerRequest, Customer>();
            CreateMap<UpdateCustomerRequest, Customer>();
            ///

            /// Address Map
            CreateMap<CreateAddressRequest, Address>();
            CreateMap<Address, GetAddressResponse>();
            ///

            /// CustomerAddress Map
            CreateMap<CustomerAddress, GetCustomerAddressResponse>();
            CreateMap<CustomerAddress, GetCustomerAddressWithCustomerResponse>();
            ///

            /// Employee Map
            CreateMap<Employee, GetEmployeeResponse>().ReverseMap();
            CreateMap<Employee, GetEmployeeWithSpecializationsResponse>();
            CreateMap<CreateEmployeeRequest, Employee>();
            CreateMap<UpdateEmployeeRequest, Employee>();

            /// Specialization Map
            CreateMap<Specialization, GetSpecializationResponse>().ReverseMap();
            CreateMap<CreateSpecializationRequest, Specialization>();
            CreateMap<UpdateSpecializationRequest, Specialization>();

            /// ServiceRequest Map
            CreateMap<ServiceRequest, GetServiceResponse>().ReverseMap();
            CreateMap<ServiceRequest, GetServiceDetailsResponse>();
            CreateMap<ServiceRequest, GetServiceWithCustomerAndAddressResponse>();
            CreateMap<CreateServiceRequest, ServiceRequest>();
            CreateMap<UpdateServiceRequest, ServiceRequest>();
            ///

            /// Material Map
            CreateMap<Material, GetMaterialResponse>().ReverseMap();
            CreateMap<CreateMaterialRequest, Material>();
            ///

            // Quote Map
            CreateMap<Quote, GetQuoteResponse>().ReverseMap();
            CreateMap<Quote, GetQuoteWithServiceDetailsResponse>();
            CreateMap<UpdateQuoteStatusRequest, Quote>();

            // Payment Map
            CreateMap<Payment, GetPaymentResponse>().ReverseMap();
            CreateMap<CreatePaymentRequest, Quote>();
            CreateMap<UpdatePaymentStatusRequest, Quote>();

            // JobValuation Map
            CreateMap<JobValuation, GetJobValuationResponse>().ReverseMap();
            CreateMap<CreateJobValuationRequest, JobValuation>();
            CreateMap<ServiceRequestJobValuation, GetServiceRequestJobValuationResponse>().ReverseMap();
        }
    }
}
