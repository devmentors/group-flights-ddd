using GroupFlights.Sales.Application.Commands.AddOfferVariant;
using GroupFlights.Sales.Application.Commands.ConfirmVariant;
using GroupFlights.Sales.Application.Commands.RevealOffer;
using GroupFlights.Shared.Plumbing.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace GroupFlights.Sales.Application.Commands.CrossCuttings;

public static class Extensions
{
    public static void EnforceWorkloadManagementRulesInSales(this IServiceCollection services)
    {
        services.TryDecorate<ICommandHandler<AddOfferVariantCommand>,
            WorkloadManagementEnforcementCommandHandlerDecorator<AddOfferVariantCommand>>();
        
        services.TryDecorate<ICommandHandler<ConfirmOfferVariantCommand>,
            WorkloadManagementEnforcementCommandHandlerDecorator<ConfirmOfferVariantCommand>>();
        
        services.TryDecorate<ICommandHandler<RevealOfferCommand>,
            WorkloadManagementEnforcementCommandHandlerDecorator<RevealOfferCommand>>();
    }
}