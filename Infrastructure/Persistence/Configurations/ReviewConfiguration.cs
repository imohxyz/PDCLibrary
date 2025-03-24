using Cinema9.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cinema9.Infrastructure.Persistence.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        _ = builder.ToTable(nameof(MyDatabase.Reviews));

        _ = builder.Property(entity => entity.ReviewerName)
            .HasColumnType("NVARCHAR(100)");

        _ = builder.Property(entity => entity.Comment)
            .HasColumnType("NVARCHAR(2000)");

        _ = builder.HasOne(entity => entity.Movie)
            .WithMany(entity => entity.Reviews)
            .HasForeignKey(entity => entity.MovieId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
