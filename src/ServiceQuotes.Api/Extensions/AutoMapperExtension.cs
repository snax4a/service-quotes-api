using AutoMapper;
using ServiceQuotes.Application.MappingProfiles;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ServiceQuotes.Api.Extensions
{
    public static class AutoMapperExtension
    {
        public static void AddAutoMapperSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddAutoMapper(typeof(MappingProfile));
        }
    }
}
