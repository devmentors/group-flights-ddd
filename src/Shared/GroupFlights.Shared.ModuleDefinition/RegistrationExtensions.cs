using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GroupFlights.Shared.ModuleDefinition;

public static class Modules
{
    private static Dictionary<string, ModuleDefinition> RegisteredModules = new();
    private static Dictionary<RegistrationRouteKey, NaiveAccessControl> RouteToNaiveAccessControl = new();
    
    public static void RegisterModule<TModule>(Func<TModule> moduleFactory = default) where TModule : ModuleDefinition
    {
        var moduleDefinition = moduleFactory is not null
            ? moduleFactory()
            : Activator.CreateInstance<TModule>();
        
        RegisteredModules.Add(moduleDefinition.ModuleName, moduleDefinition);
    }

    public static void AddModules(this IServiceCollection services, IConfiguration configuration)
    {
        foreach (var module in RegisteredModules.Values)
        {
            module.AddDependencies(services, configuration);
        }
    }

    public static void MapModulesEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        foreach (var module in RegisteredModules.Values)
        {
            foreach (var endpoint in module.CreateEndpoints(endpointRouteBuilder))
            {
                var endpointRoute = module.GetModulePrefixSanitized() + endpoint.WithLeadingSlash().Pattern;
                RouteToNaiveAccessControl.Add(new RegistrationRouteKey(endpointRoute, endpoint.HttpVerb), endpoint.AccessControl);
                
                endpointRouteBuilder.MapMethods(
                    pattern: endpointRoute,
                    httpMethods: new[] { endpoint.HttpVerb.ToString().ToUpper() },
                    handler: endpoint.Handler);
            }
        }
    }

    public static NaiveAccessControl ResolveAccessFor(RegistrationRouteKey route)
    {
        return RouteToNaiveAccessControl[route];
    }

    private static string GetModulePrefixSanitized(this ModuleDefinition moduleDefinition)
    {
        var modulePrefix = moduleDefinition.ModulePrefix;
        
        if (modulePrefix[0] != '/')
        {
            modulePrefix = '/' + modulePrefix;
        }

        if (modulePrefix[^1] == '/')
        {
            modulePrefix = new string(modulePrefix.Substring(0, modulePrefix.Length - 1));
        }

        return modulePrefix;
    }

    private static EndpointRegistration WithLeadingSlash(this EndpointRegistration endpointRegistration)
    {
        if (endpointRegistration.Pattern.Length == 0)
        {
            return endpointRegistration;
        }

        if (endpointRegistration.Pattern[0] != '/')
        {
            endpointRegistration = endpointRegistration with { Pattern = '/' + endpointRegistration.Pattern };
        }

        return endpointRegistration;
    }
}

public record RegistrationRouteKey(string RoutePattern, HttpVerb HttpVerb);