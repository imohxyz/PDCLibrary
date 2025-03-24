using Cinema9.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cinema9.Infrastructure.Persistence.Configurations;

public class MovieConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        _ = builder.ToTable(nameof(MyDatabase.Movies));

        _ = builder.Property(entity => entity.Title)
            .HasColumnType("NVARCHAR(100)");

        _ = builder.Property(entity => entity.Synopsis)
            .HasColumnType("NVARCHAR(2000)");

        _ = builder.Property(entity => entity.Budget)
            .HasColumnType("MONEY");

        _ = builder.HasOne(entity => entity.Country)
            .WithMany(entity => entity.Movies)
            .HasForeignKey(entity => entity.CountryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
