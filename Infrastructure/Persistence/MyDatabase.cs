using Cinema9.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Reflection;

namespace Cinema9.Infrastructure.Persistence;

public class MyDatabase(DbContextOptions<MyDatabase> options) : DbContext(options)
// public class MyDatabase : IdentityDbContext<AppUser, AppRole, Guid,
//     IdentityUserClaim<Guid>, IdentityUserRole<Guid>, IdentityUserLogin<Guid>,
//     IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
{
    public DbSet<Country> Countries => Set<Country>();
    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<Rent> Rents => Set<Rent>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.HasDefaultSchema("cinema9");
        // Configure Identity tables
        _ = modelBuilder.Entity<AppUser>(b =>
        {
            b.ToTable("Users");
            b.HasMany(e => e.Claims)
                .WithOne()
                .HasForeignKey(uc => uc.UserId)
                .IsRequired();
            
            b.HasMany(e => e.Logins)
                .WithOne()
                .HasForeignKey(ul => ul.UserId)
                .IsRequired();
            
            b.HasMany(e => e.Tokens)
                .WithOne()
                .HasForeignKey(ut => ut.UserId)
                .IsRequired();
            
            b.HasMany(e => e.UserRoles)
                .WithOne()
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
        });

        _ = modelBuilder.Entity<AppRole>(b =>
        {
            b.ToTable("Roles");
            b.HasMany(e => e.UserRoles)
                .WithOne()
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();
            
            b.HasMany(e => e.RoleClaims)
                .WithOne()
                .HasForeignKey(rc => rc.RoleId)
                .IsRequired();
        });

        _ = modelBuilder.Entity<IdentityUserClaim<Guid>>(b =>
        {
            b.ToTable("UserClaims");
            b.HasKey(uc => uc.Id);
        });

        _ = modelBuilder.Entity<IdentityUserLogin<Guid>>(b =>
        {
            b.ToTable("UserLogins");
            b.HasKey(l => new { l.LoginProvider, l.ProviderKey });
        });

        _ = modelBuilder.Entity<IdentityUserToken<Guid>>(b =>
        {
            b.ToTable("UserTokens");
            b.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });
        });

        _ = modelBuilder.Entity<IdentityRoleClaim<Guid>>(b =>
        {
            b.ToTable("RoleClaims");
            b.HasKey(rc => rc.Id);
        });

        _ = modelBuilder.Entity<IdentityUserRole<Guid>>(b =>
        {
            b.ToTable("UserRoles");
            b.HasKey(r => new { r.UserId, r.RoleId });
        });

        _ = modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // _ = modelBuilder.Entity<AppUser>().ToTable("Users");
        // _ = modelBuilder.Entity<AppRole>().ToTable("Roles");
        // _ = modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
        // _ = modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles");
        // _ = modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins");
        // _ = modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims");
        // _ = modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens");

        base.OnModelCreating(modelBuilder);
    }
}
