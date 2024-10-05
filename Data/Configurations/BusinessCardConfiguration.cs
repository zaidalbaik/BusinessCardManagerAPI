using BusinessCardManagerAPI.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessCardManagerAPI.Data.Configurations
{
    public class BusinessCardConfiguration : IEntityTypeConfiguration<BusinessCard>
    {
        public void Configure(EntityTypeBuilder<BusinessCard> builder)
        {
            builder.HasKey(so => so.Id);

            builder.Property(u => u.Id)
                   .ValueGeneratedOnAdd()
                   .HasDefaultValueSql("NEWID()");

            builder.Property(bc => bc.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(bc => bc.Gender)
                   .IsRequired()
                   .HasMaxLength(10);

            builder.Property(bc => bc.DateOfBirth)
                   .IsRequired();

            builder.Property(bc => bc.Email)
                   .IsRequired()
                   .HasMaxLength(255);
             
            builder.Property(bc => bc.Phone)
                   .IsRequired()
                   .HasMaxLength(20);
        
            builder.Property(bc => bc.Address)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.HasQueryFilter(e => e.IsDeleted == false);

            builder.HasIndex(u => u.Id).IsUnique();
            builder.HasIndex(bc => bc.Email).IsUnique();
        }
    }
}
