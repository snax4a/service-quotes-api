using AutoMapper;
using ServiceQuotes.Application.DTOs.Account;
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
        }
    }
}
