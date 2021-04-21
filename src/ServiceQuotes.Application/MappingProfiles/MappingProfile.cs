using AutoMapper;
using ServiceQuotes.Application.DTOs.Account;
using ServiceQuotes.Application.DTOs.Customer;
using ServiceQuotes.Domain.Entities;

namespace ServiceQuotes.Application.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            /// Account Map
            CreateMap<Account, GetAccountDTO>().ReverseMap();
            CreateMap<CreateAccountDTO, Account>();
            CreateMap<UpdateAccountDTO, Account>();
            CreateMap<Account, AuthenticatedAccountDTO>();
            ///

            /// Customer Map
            CreateMap<Customer, GetCustomerDTO>().ReverseMap();
            CreateMap<CreateCustomerDTO, Customer>();
            CreateMap<UpdateCustomerDTO, Customer>();
            ///
        }
    }
}
