using Cinema9.Infrastructure.Persistence;
using Cinema9.Infrastructure.Persistence.Seedings;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

namespace Cinema9.Infrastructure;

public static class ConfigureInfrastructure
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MyDatabase");

        _ = services.AddDbContext<MyDatabase>(options =>
        {
            _ = options.UseSqlServer(connectionString, builder =>
            {
                _ = builder.MigrationsAssembly(typeof(MyDatabase).Assembly.FullName);
                _ = builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            });

            _ = options.ConfigureWarnings(wcb => wcb.Ignore(CoreEventId.RowLimitingOperationWithoutOrderByWarning));
            _ = options.ConfigureWarnings(wcb => wcb.Throw(RelationalEventId.MultipleCollectionIncludeWarning));
        });

        _ = services.AddScoped<DatabaseMigrator>();
        _ = services.AddScoped<DataSeeder>();
    }

    public static async Task InitializeDatabase(this IHost host)
    {
        using var serviceScope = host.Services.CreateScope();
        var serviceProvider = serviceScope.ServiceProvider;

        var databaseMigrator = serviceProvider.GetRequiredService<DatabaseMigrator>();
        await databaseMigrator.Migrate(serviceProvider);

        var dataSeeder = serviceProvider.GetRequiredService<DataSeeder>();
        await dataSeeder.SeedData();
    }
}
