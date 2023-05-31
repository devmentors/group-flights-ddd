namespace GroupFlights.Shared.ModuleDefinition;

public record EndpointRegistration(
    string Pattern,
    HttpVerb HttpVerb,
    NaiveAccessControl AccessControl,
    Delegate Handler);
    
public enum HttpVerb
{
    GET,
    POST,
    PUT,
    DELETE
}

public enum NaiveAccessControl
{
    Anonymous = 0,
    ClientOnly = 1,
    CashierOnly = 2,
    ClientAndCashier = ClientOnly | CashierOnly,
    AdministratorOnly = 64
}