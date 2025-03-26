using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using QuestPDF.Infrastructure;

namespace Cinema9.Logics;

public static class ConfigureLogics
{
    public static void AddLogics(this IServiceCollection services)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });
    }
}
