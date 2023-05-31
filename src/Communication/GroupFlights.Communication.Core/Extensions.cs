using GroupFlights.Communication.Core.ModuleApi;
using GroupFlights.Communication.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GroupFlights.Communication.Core;

public static class Extensions
{
    public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ICommunicationApi, CommunicationApi>();

        return services;
    }
}