using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceQuotes.Domain.Entities;

namespace ServiceQuotes.Infrastructure.Context.EntityConfigurations
{
    public class EmployeeSpecializationEntityConfiguration : IEntityTypeConfiguration<EmployeeSpecialization>
    {
        public void Configure(EntityTypeBuilder<EmployeeSpecialization> builder)
        {
            builder.ToTable("EmployeeSpecializations");

            builder.HasKey(es => new { es.EmployeeId, es.SpecializationId });

            builder
                .HasOne(es => es.Employee)
                .WithMany(e => e.EmployeeSpecializations)
                .HasForeignKey(es => es.EmployeeId);

            builder
                .HasOne(es => es.Specialization)
                .WithMany(s => s.EmployeeSpecializations)
                .HasForeignKey(es => es.SpecializationId);
        }
    }
}
