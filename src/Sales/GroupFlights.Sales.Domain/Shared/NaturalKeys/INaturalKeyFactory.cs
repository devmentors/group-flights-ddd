namespace GroupFlights.Sales.Application.NaturalKeys;

public interface INaturalKeyFactory<T>
{
    string CreateNaturalKey();
}