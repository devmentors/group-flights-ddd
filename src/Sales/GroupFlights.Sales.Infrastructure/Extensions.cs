using GroupFlights.Sales.Application.DeadlineRegistry;
using GroupFlights.Sales.Application.NaturalKeys;
using GroupFlights.Sales.Domain.Offers;
using GroupFlights.Sales.Domain.Offers.Repositories;
using GroupFlights.Sales.Domain.Reservations.Repositories;
using GroupFlights.Sales.Infrastructure.Data.EF;
using GroupFlights.Sales.Infrastructure.Data.Registries;
using GroupFlights.Sales.Infrastructure.Data.Repositories;
using GroupFlights.Sales.Infrastructure.NaturalKeys;
using GroupFlights.Shared.Plumbing.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GroupFlights.Sales.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IOfferRepository, OfferRepository>();
        services.AddScoped<IReservationRepository, ReservationRepository>();
        services.AddScoped<INaturalKeyFactory<OfferDraft>, OfferNumberFactory>();
        services.AddPostgres<SalesDbContext>(configuration);
        services.AddScoped<IDeadlineRegistry, DeadlineRegistry>();

        return services;
    }
}