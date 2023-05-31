namespace GroupFlights.Shared.Plumbing.UserContext;

public interface IUserContextAccessor
{
    IUserContext Get();
}