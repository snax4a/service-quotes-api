﻿using ServiceQuotes.Domain.Entities.Enums;

namespace ServiceQuotes.Application.Filters
{
    public class GetAccountsFilter : SearchStringFilter
    {
        public string Email { get; set; }
        public Role? Role { get; set; }

    }
}
