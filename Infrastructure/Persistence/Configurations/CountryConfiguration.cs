using Cinema9.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cinema9.Infrastructure.Persistence.Configurations;

public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        _ = builder.ToTable(nameof(MyDatabase.Countries));

        _ = builder.Property(entity => entity.Code)
            .HasColumnType("NVARCHAR(5)");

        _ = builder.Property(entity => entity.Name)
            .HasColumnType("NVARCHAR(100)");
    }
}
