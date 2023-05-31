using System.Linq.Expressions;
using GroupFlights.Sales.Domain.Reservations.Passengers;
using GroupFlights.Shared.Types.Specification;

namespace GroupFlights.Sales.Domain.Reservations.Specifications;

public class EverythingProvidedToConfirmReservation : Specification<UnconfirmedReservation>
{
    protected override Expression<Func<UnconfirmedReservation, bool>> AsPredicateExpression()
    {
        return reservation => PassengersProvidedOnTime(reservation) && ContractSigned(reservation);
    }

    private bool PassengersProvidedOnTime(UnconfirmedReservation unconfirmedReservation)
    {
        if (unconfirmedReservation.PassengerNamesRequiredImmediately is false)
        {
            return true;
        }

        var declaredCount = unconfirmedReservation.DeclaredPassengers.TotalCount;
        var providedCount = (unconfirmedReservation.ProvidedPassengers ?? new List<Passenger>()).Count;
        
        return declaredCount == providedCount;
    }

    private bool ContractSigned(UnconfirmedReservation unconfirmedReservation)
    {
        if (unconfirmedReservation.ContractToSign.Signed is true)
        {
            return true;
        }

        return false;
    }
}