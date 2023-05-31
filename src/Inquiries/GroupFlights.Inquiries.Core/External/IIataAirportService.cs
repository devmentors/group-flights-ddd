using GroupFlights.Shared.Types;

namespace GroupFlights.Inquiries.Core.External;

public interface IIataAirportService
{
    Task<IReadOnlyCollection<Airport>> GetAirportsByQueryPhrase(string queryPhrase,
        CancellationToken cancellationToken = default);

    Task<Airport> GetAirportByCode(IataAirportCode code, CancellationToken cancellationToken = default);

}