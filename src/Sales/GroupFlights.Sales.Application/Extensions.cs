using GroupFlights.Sales.Application.ApplicationServices;
using GroupFlights.Sales.Application.Commands.CrossCuttings;
using GroupFlights.Sales.Application.DeadlineRegistry;
using GroupFlights.Sales.Application.ModuleApi;
using GroupFlights.Sales.Application.PaymentRegistry;
using GroupFlights.Sales.Domain.Reservations.DomainServices;
using GroupFlights.Sales.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GroupFlights.Sales.Application;

public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ISalesApi, SalesApi>();
        services.AddScoped<ReservationConfirmationService>();
        services.AddScoped<IPaymentRegistry, PaymentRegistry.PaymentRegistry>();
        services.AddScoped<ReservationConfirmationDomainService>();
        
        return services;
    }
}