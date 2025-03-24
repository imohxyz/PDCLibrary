using Cinema9.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace Cinema9.Infrastructure;

public static class ConfigureInfrastructure
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<MyDatabase>();
    }
}
