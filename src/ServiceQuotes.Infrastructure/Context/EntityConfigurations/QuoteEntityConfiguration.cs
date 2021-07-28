using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceQuotes.Domain.Entities;

namespace ServiceQuotes.Infrastructure.Context.EntityConfigurations
{
    public class QuoteEntityConfiguration : IEntityTypeConfiguration<Quote>
    {
        public void Configure(EntityTypeBuilder<Quote> builder)
        {
            builder.HasKey(quote => quote.Id);

            builder.Property(quote => quote.Id).ValueGeneratedOnAdd().IsRequired();

            builder.Property(quote => quote.ReferenceNumber).ValueGeneratedOnAdd().IsRequired();

            builder.Property(quote => quote.Total).HasPrecision(7, 2).IsRequired();

            builder.Property(quote => quote.Created).HasDefaultValueSql("NOW()").IsRequired();

            builder.HasOne(quote => quote.ServiceRequest).WithOne().HasForeignKey<Quote>("ServiceRequestId").IsRequired();

            builder.HasIndex(quote => quote.ReferenceNumber).IsUnique();
        }
    }
}
