using ServiceQuotes.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceQuotes.Application.Filters
{
    public class GetHeroesFilter
    {
        public string Name { get; set; }
        public string Nickname { get; set; }

        public int? Age { get; set; }

        public string Individuality { get; set; }
        public HeroType? HeroType { get; set; }

        public string Team { get; set; }

    }
}
