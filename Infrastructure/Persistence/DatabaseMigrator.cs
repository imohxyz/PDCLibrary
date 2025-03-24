using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Cinema9.Infrastructure.Persistence;

public class DatabaseMigrator(ILogger<DatabaseMigrator> logger)
{
    public async Task Migrate(IServiceProvider serviceProvider)
    {
        var myDatabase = serviceProvider.GetRequiredService<MyDatabase>();

        var pendingMigrations = await myDatabase.Database.GetPendingMigrationsAsync();

        if (pendingMigrations.Any())
        {
            logger.LogInformation("Applying database migration...");

            await myDatabase.Database.MigrateAsync();
        }
        else
        {
            logger.LogInformation("Database is up to date. No database migration required.");
        }
    }
}
