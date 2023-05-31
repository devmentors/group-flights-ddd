using GroupFlights.Backoffice.Core.Data;
using GroupFlights.Backoffice.Core.DocumentGeneration;
using GroupFlights.Backoffice.Core.ModuleApi;
using GroupFlights.Backoffice.Core.Repositories;
using GroupFlights.Backoffice.Core.Services;
using GroupFlights.Backoffice.Shared;
using GroupFlights.Shared.Plumbing.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GroupFlights.Backoffice.Core;

public static class Extensions
{
    public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IBackofficeApi, BackofficeApi>();
        services.AddScoped<IDocumentService, DocumentService>();
        services.AddScoped<IFileRepository, FileRepository>();
        services.AddSingleton<ContractGenerator>();
        services.AddPostgres<BackofficeDbContext>(configuration);

        return services;
    }
}