using GroupFlights.Inquiries.Core.Data.EF;
using GroupFlights.Inquiries.Core.External;
using GroupFlights.Inquiries.Core.Repositories;
using GroupFlights.Inquiries.Core.Services;
using GroupFlights.Inquiries.Core.Validators;
using GroupFlights.Shared.Plumbing.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GroupFlights.Inquiries.Core;

internal static class Extensions
{
    public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IInquiryService, InquiryService>();
        services.AddScoped<IInquiryRepository, InquiryRepository>();
        services.AddScoped<IIataAirportService, JsonBasedIataAirportService>();
        services.AddScoped<InquiryValidator>();
        services.AddPostgres<InquiriesDbContext>(configuration);

        return services;
    }
}