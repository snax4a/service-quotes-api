using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceQuotes.Domain.Entities;

namespace ServiceQuotes.Infrastructure.Context.EntityConfigurations
{
    public class PaymentEntityConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(payment => payment.Id);

            builder.Property(payment => payment.Id).ValueGeneratedOnAdd().IsRequired();

            builder.Property(payment => payment.Provider).HasMaxLength(50).IsRequired();

            builder.Property(payment => payment.TransactionId).HasMaxLength(150).IsRequired(false);

            builder.Property(payment => payment.Amount).HasPrecision(7, 2).IsRequired();

            builder.Property(payment => payment.Created).IsRequired();

            builder.Property(payment => payment.Updated).IsRequired(false);

            builder
                .HasOne(payment => payment.Quote)
                .WithMany(quote => quote.Payments)
                .HasForeignKey(payment => payment.QuoteId);

            builder
                .HasOne(payment => payment.Customer)
                .WithMany(customer => customer.Payments)
                .HasForeignKey(payment => payment.CustomerId);
        }
    }
}
