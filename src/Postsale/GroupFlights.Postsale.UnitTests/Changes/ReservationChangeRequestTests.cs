using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GroupFlights.Postsale.Domain.Changes.DomainService;
using GroupFlights.Postsale.Domain.Changes.Events;
using GroupFlights.Postsale.Domain.Changes.Repositories;
using GroupFlights.Postsale.Domain.Changes.Request;
using GroupFlights.Postsale.Domain.Exceptions;
using GroupFlights.Sales.Shared;
using GroupFlights.Sales.Shared.Changes;
using GroupFlights.Shared.Types;
using NSubstitute;
using Xunit;

namespace GroupFlights.Postsale.UnitTests.Changes;

public class ReservationChangeRequestTests
{
    private ISalesApi _salesApi = Substitute.For<ISalesApi>();
    private IReservationChangeRequestRepository _repository = Substitute.For<IReservationChangeRequestRepository>();

    private ReservationChangeRequestDomainService _domainService;

    [Fact]
    public async Task ReservationChangeRequestDomainService_ForbidsCreatingChangeRequest_IfThereIsOtherActiveForThisUser()
    {
        _repository.ExistsActiveForGivenUser(Arg.Any<UserId>(), Arg.Any<CancellationToken>())
            .ReturnsForAnyArgs(true);

        var reservationId = Guid.NewGuid();
        var currentTravelDate = DateTime.UtcNow.AddDays(10);
        
        _salesApi.GetReservationForChange(Arg.Any<GetReservationForChangeQuery>(), Arg.Any<CancellationToken>())
            .ReturnsForAnyArgs(new ReservationToChangeDto(
                reservationId,
                AirlineType.Traditional,
                true,
                new ReservationCostDto(new Money(250, Currency.PLN), new Money(50, Currency.PLN)),
                new []
                {
                    new FlightSegmentDto(
                        currentTravelDate,
                        new Airport("WAW", "Warsaw", "Warsaw Chopin", "Poland"),
                        new Airport("BCN", "Barcelona", "Barcelona Airport", "Spain"),
                        new FlightTimeDto(3, 15)
                    )
                },
                new [] { new RequiredPaymentDto(Guid.NewGuid(), new DeadlineDto(DateTime.UtcNow.AddDays(1), null, Guid.NewGuid())) },
                new DeadlineDto(DateTime.UtcNow.AddDays(1), null, Guid.NewGuid())
            ));

        _domainService = new(_salesApi, _repository);

        await Assert.ThrowsAsync<OnlyOneActiveChangePerReservationIsAllowedException>(async () =>
        {
            await _domainService.CreateChangeRequest(
                reservationId,
                currentTravelDate.AddDays(10),
                new UserId(Guid.NewGuid()));
        });
    }
    
    [Fact]
    public async Task ReservationChangeRequest_ChangesStatus_OnReject()
    {
        var changeRequest = await PrepareChangeRequest();
        
        changeRequest.RejectChange();

        var emittedEvent = changeRequest.DomainEvents
            .OfType<ReservationChangeRequestFinalized>().SingleOrDefault();

        Assert.Equal(expected: ReservationChangeRequest.CompletionStatus.ChangeRejectedByRequester,
            emittedEvent?.CompletionStatus);
    }

    private async Task<ReservationChangeRequest> PrepareChangeRequest()
    {
        _repository.ExistsActiveForGivenUser(Arg.Any<UserId>(), Arg.Any<CancellationToken>())
            .ReturnsForAnyArgs(false);

        var reservationId = Guid.NewGuid();
        var currentTravelDate = DateTime.UtcNow.AddDays(10);

        _salesApi.GetReservationForChange(Arg.Any<GetReservationForChangeQuery>(), Arg.Any<CancellationToken>())
            .ReturnsForAnyArgs(new ReservationToChangeDto(
                reservationId,
                AirlineType.Traditional,
                true,
                new ReservationCostDto(new Money(250, Currency.PLN), new Money(50, Currency.PLN)),
                new[]
                {
                    new FlightSegmentDto(
                        currentTravelDate,
                        new Airport("WAW", "Warsaw", "Warsaw Chopin", "Poland"),
                        new Airport("BCN", "Barcelona", "Barcelona Airport", "Spain"),
                        new FlightTimeDto(3, 15)
                    )
                },
                new[]
                {
                    new RequiredPaymentDto(Guid.NewGuid(),
                        new DeadlineDto(DateTime.UtcNow.AddDays(1), null, Guid.NewGuid()))
                },
                new DeadlineDto(DateTime.UtcNow.AddDays(1), null, Guid.NewGuid())
            ));

        _domainService = new(_salesApi, _repository);

        return await _domainService.CreateChangeRequest(
            reservationId,
            currentTravelDate.AddDays(10),
            new UserId(Guid.NewGuid()));
    }
}