﻿using ServiceQuotes.Domain.Entities.Enums;
using System;

namespace ServiceQuotes.Application.DTOs.Hero
{
    public class GetHeroDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Nickname { get; set; }

        public int? Age { get; set; }

        public string Individuality { get; set; }
        public HeroType? HeroType { get; set; }

        public string Team { get; set; }

    }
}
