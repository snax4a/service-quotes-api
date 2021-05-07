using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceQuotes.Domain.Entities;
using ServiceQuotes.Domain.Entities.Enums;
using System;

namespace ServiceQuotes.Infrastructure.Context.EntityConfigurations
{
    public class QuoteEntityConfiguration : IEntityTypeConfiguration<Quote>
    {
        public void Configure(EntityTypeBuilder<Quote> builder)
        {
            builder.HasKey(quote => quote.Id);

            builder.Property(quote => quote.Id).ValueGeneratedOnAdd().IsRequired();

            builder.Property(quote => quote.ReferenceNumber).IsRequired();

            builder.Property(quote => quote.Total).HasPrecision(7,2).IsRequired();

            builder.HasIndex("QuoteId").IsUnique();
        }
    }
}
