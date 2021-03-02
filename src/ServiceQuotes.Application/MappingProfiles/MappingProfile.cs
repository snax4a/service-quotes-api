using AutoMapper;
using ServiceQuotes.Application.DTOs.Hero;
using ServiceQuotes.Application.DTOs.Account;
using ServiceQuotes.Domain.Entities;

namespace ServiceQuotes.Application.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            /// Hero Map
            CreateMap<Hero, GetHeroDTO>().ReverseMap();
            CreateMap<InsertHeroDTO, Hero>();
            CreateMap<UpdateHeroDTO, Hero>();
            ///

            /// Account Map
            CreateMap<Account, GetAccountDTO>().ReverseMap();
            CreateMap<CreateAccountDTO, Account>();
            CreateMap<UpdateAccountDTO, Account>();
            CreateMap<Account, AuthenticatedAccountDTO>();
            ///
        }
    }
}
