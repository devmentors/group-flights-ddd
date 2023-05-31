using GroupFlights.Finance.Core.Data;
using GroupFlights.Finance.Core.ModuleApi;
using GroupFlights.Finance.Core.Services;
using GroupFlights.Finance.Core.Validators;
using GroupFlights.Finance.Shared;
using GroupFlights.Shared.Plumbing.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GroupFlights.Finance.Core;

public static class Extensions
{
    public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IFinanceApi, FinanceApi>();
        services.AddScoped<IPayerService, PayerService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddSingleton<PayerValidator>();
        services.AddSingleton<PaymentValidator>();
        services.AddPostgres<FinanceDbContext>(configuration);

        return services;
    }
}