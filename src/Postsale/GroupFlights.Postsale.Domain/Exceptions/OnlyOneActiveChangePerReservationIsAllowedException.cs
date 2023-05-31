using GroupFlights.Shared.Types.Exceptions;

namespace GroupFlights.Postsale.Domain.Exceptions;

public class OnlyOneActiveChangePerReservationIsAllowedException : HumanPresentableException
{
    public OnlyOneActiveChangePerReservationIsAllowedException()
        : base("Tylko jedna aktywna zmiana jest dozwolona dla kazdej rezerwacji", ExceptionCategory.ValidationError)
    {
    }
}