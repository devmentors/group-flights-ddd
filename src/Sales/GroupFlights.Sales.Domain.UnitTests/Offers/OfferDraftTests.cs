using System;
using System.Collections.Generic;
using GroupFlights.Sales.Domain.Offers;
using GroupFlights.Sales.Domain.Offers.Exceptions;
using GroupFlights.Sales.Domain.Offers.Shared;
using GroupFlights.Sales.Domain.Shared;
using GroupFlights.Sales.Domain.Shared.External;
using GroupFlights.Sales.Shared;
using GroupFlights.Shared.Types;
using GroupFlights.Shared.Types.Time;
using Xunit;

namespace GroupFlights.Sales.Domain.UnitTests.Offers;

public class OfferDraftTests
{
    private TestClock _clock = new();

    private SalesConfiguration _salesConfiguration = new(
        DefaultOfferValidTime: TimeSpan.FromDays(2),
        ContractSignTime: TimeSpan.FromSeconds(0));

    private CashierBufferConfiguration _bufferConfiguration = new(
        MinimalBufferInHours: 6);

    [Fact]
    public void OfferDraft_AddNewVariant_DoesNotAllowOverdueVariant()
    {
        _clock.SetUtcNow(DateTime.UtcNow.AddDays(31));
        
        var draft = PrepareDraft();
        var flightSegment = PrepareSegmentFromRequestedTravel(draft.RequestedTravel);

        Assert.Throws<CannotAddAlreadyOverdueVariant>(() =>
        {
            draft.AddNewVariant(
                AirlineType.Traditional,
                "Imaginary Airlines",
                new[] { flightSegment },
                null,
                new("IMG/OFF/123"),
                _clock,
                _salesConfiguration,
                _bufferConfiguration,
                _clock.UtcNow.AddHours(-1));
        });
    }

    private FlightSegment PrepareSegmentFromRequestedTravel(RequestedTravel requestedTravel)
    {
        return new FlightSegment(requestedTravel.Travel.Date.AddDays(1),
            requestedTravel.Travel.SourceAirport,
            requestedTravel.Travel.TargetAirport,
            new FlightTime(3, 0));
    }
    
    private OfferDraft PrepareDraft()
    {
        var offerSource = new OfferSource("Test", "This test");
        var client = new Client("Test", "Testable", new Email("test@examaple.com"), "123-234-345");
        var requestedTravel = new RequestedTravel(
            new RequestedTravel.Flight(_clock.UtcNow.AddDays(60),
                new Airport("WAW", "Warsaw", "Warsaw Chopin", "Poland"),
                new Airport("BCN", "Barcelona", "Barcelona Airport", "Spain")),
            null,
            new PassengersData(0, 5, 10),
            false,
            false,
            null,
            _clock);

        var priorities = new List<PriorityChoice>
        {
            new(PriorityChoice.PrioritizedFeature.Flexibility, 1),
            new(PriorityChoice.PrioritizedFeature.Price, 2)
        };

        return new OfferDraft("OFR/1234", offerSource, client, requestedTravel, priorities);
    }

    private class TestClock : IClock
    {
        private DateTime _utcNow;

        public DateTime UtcNow => _utcNow;

        public void SetUtcNow(DateTime dateTime)
        {
            _utcNow = dateTime;
        }
    }
}