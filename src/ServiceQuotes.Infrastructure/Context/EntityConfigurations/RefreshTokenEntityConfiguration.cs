using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceQuotes.Domain.Entities;

namespace ServiceQuotes.Infrastructure.Context.EntityConfigurations
{
    public class RefreshTokenEntityConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(rt => rt.Id);

            builder.Property(rt => rt.Id).ValueGeneratedOnAdd().IsRequired();

            builder.Property(rt => rt.Token).IsRequired();

            builder.Property(rt => rt.Expires).IsRequired();

            builder.Property(rt => rt.Created).IsRequired();

            builder.Property(rt => rt.CreatedByIp).IsRequired();

            builder.Property(rt => rt.Revoked).IsRequired(false);

            builder.Property(rt => rt.RevokedByIp).IsRequired(false);

            builder.Property(rt => rt.ReplacedByToken).IsRequired(false);

            builder
                .HasOne(rt => rt.Account)
                .WithMany(a => a.RefreshTokens)
                .HasForeignKey(rt => rt.AccountId);
        }
    }
}
