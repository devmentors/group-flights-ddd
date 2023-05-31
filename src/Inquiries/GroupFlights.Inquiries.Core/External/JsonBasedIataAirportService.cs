using System.Collections.Immutable;
using System.Reflection;
using System.Text.Json;
using GroupFlights.Shared.Types;
using GroupFlights.Shared.Types.Exceptions;

namespace GroupFlights.Inquiries.Core.External;

internal class JsonBasedIataAirportService : IIataAirportService
{
    private static ImmutableDictionary<string,Airport> _codeToAirport;

    private static ImmutableDictionary<string, Airport> CodeToAirport
    {
        get
        {
            if (_codeToAirport is not null)
            {
                return _codeToAirport;
            }

            var preparedMap = new Dictionary<string, Airport>();

            var assembly = typeof(JsonBasedIataAirportService).Assembly;

            foreach (var resourceName in assembly.GetManifestResourceNames()
                         .Where(str => str.Contains("airports_") && str.EndsWith(".json")))
            {
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                using (StreamReader reader = new StreamReader(stream))
                {
                    var json = reader.ReadToEndAsync().GetAwaiter().GetResult();
                    var airports = JsonSerializer.Deserialize<SerializedAirport[]>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    foreach (var airport in airports)
                    {
                        preparedMap.Add(airport.Code, new Airport(airport.Code, airport.City, airport.Name, airport.Country));
                    }
                }   
            }

            _codeToAirport = preparedMap.ToImmutableDictionary();

            return _codeToAirport;
        }
    }

    public Task<IReadOnlyCollection<Airport>> GetAirportsByQueryPhrase(string queryPhrase, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyCollection<Airport>>(CodeToAirport.Values
            .Where(a => a.Name.ToLower().Contains(queryPhrase.ToLower()) || a.City.ToLower().Contains(queryPhrase.ToLower()))
            .ToList());
    }

    public Task<Airport> GetAirportByCode(IataAirportCode code, CancellationToken cancellationToken = default)
    {
        CodeToAirport.TryGetValue(code, out var airport);

        if (airport is null)
        {
            throw new DoesNotExistException();
        }

        return Task.FromResult(airport);
    }

    private class SerializedAirport
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}