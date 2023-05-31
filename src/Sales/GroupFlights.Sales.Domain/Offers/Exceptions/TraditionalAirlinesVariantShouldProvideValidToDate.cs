using GroupFlights.Shared.Types.Exceptions;

namespace GroupFlights.Sales.Domain.Offers.Exceptions;

public class TraditionalAirlinesVariantShouldProvideValidToDate : HumanPresentableException
{
    public TraditionalAirlinesVariantShouldProvideValidToDate() 
        : base("Dodanie wariantu oferty linii tradycyjnych wymaga podania daty jej ważności otrzymanej z GDS",
            ExceptionCategory.ValidationError)
    {
    }
}