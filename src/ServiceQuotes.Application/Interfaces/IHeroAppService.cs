﻿using ServiceQuotes.Application.DTOs.Hero;
using ServiceQuotes.Application.Filters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceQuotes.Application.Interfaces
{
    public interface IHeroAppService : IDisposable
    {
        #region Hero Methods
        public Task<List<GetHeroDTO>> GetAllHeroes(GetHeroesFilter filter);

        public Task<GetHeroDTO> GetHeroById(Guid id);

        public Task<GetHeroDTO> CreateHero(InsertHeroDTO hero);

        public Task<GetHeroDTO> UpdateHero(Guid id, UpdateHeroDTO updatedHero);

        public Task<bool> DeleteHero(Guid id);

        #endregion
    }
}
