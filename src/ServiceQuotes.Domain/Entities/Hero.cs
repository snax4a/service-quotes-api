using ServiceQuotes.Domain.Core.Entities;
using ServiceQuotes.Domain.Entities.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceQuotes.Domain.Entities
{
    public class Hero : Entity
    {
        [Required]
        public string Name { get; set; }
        public string Nickname { get; set; }
        public string Individuality { get; set; }
        public int? Age { get; set; }

        [Required]
        public HeroType? HeroType { get; set; }

        public string Team { get; set; }
    }
}
