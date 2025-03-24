using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Cinema9.Logics;

public static class ConfigureLogics
{
    public static void AddLogics(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });
    }
}
