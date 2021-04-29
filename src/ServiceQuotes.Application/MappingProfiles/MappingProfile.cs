using AutoMapper;
using ServiceQuotes.Application.DTOs.Account;
using ServiceQuotes.Application.DTOs.Customer;
using ServiceQuotes.Application.DTOs.CustomerAddress;
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
        }
    }
}
