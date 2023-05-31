using GroupFlights.Postsale.Application.Repositories;
using GroupFlights.Postsale.Domain.Changes.Repositories;
using GroupFlights.Postsale.Infrastructure.Data.EF;
using GroupFlights.Postsale.Infrastructure.Repositories;
using GroupFlights.Shared.Plumbing.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GroupFlights.Postsale.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IReservationChangeRequestRepository, ReservationChangeRequestRepository>();
        services.AddScoped<IChangeRequestsReadOnlyRepository, ChangeRequestsReadOnlyRepository>();
        services.AddPostgres<PostsaleDbContext>(configuration);
        services.AddPostgres<PostsaleReadDbContext>(configuration);

        return services;
    }
}