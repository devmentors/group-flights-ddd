using GroupFlights.Shared.Types;
using GroupFlights.Shared.Types.Exceptions;

namespace GroupFlights.Sales.Domain.Offers.Variants;

public class CalculatedTravelCost
{
    private CalculatedTravelCost()
    {
        
    }
    
    internal CalculatedTravelCost(Money airportCharges, Money netPrice, Money profitFee)
    {
        AirportCharges = airportCharges ?? throw new ArgumentNullException(nameof(airportCharges));
        NetPrice = netPrice ?? throw new ArgumentNullException(nameof(netPrice));
        ProfitFee = profitFee ?? throw new ArgumentNullException(nameof(profitFee));
    }

    internal Money CalculateTotal()
    {
        var distinctCurrencies = new HashSet<Currency>(
            new[] { AirportCharges.Currency, NetPrice.Currency, ProfitFee.Currency });
        
        var isSingleCurrencyCost = distinctCurrencies.Count == 1;

        if (isSingleCurrencyCost is false)
        {
            throw new TravelCostNotInSingleCurrencyException();
        }
        
        return new (AirportCharges.Amount + NetPrice.Amount + ProfitFee.Amount, distinctCurrencies.Single());
    }

    internal Money AirportCharges { get; init; }
    internal Money NetPrice { get; init; }
    internal Money ProfitFee { get; init; }
}

internal class TravelCostNotInSingleCurrencyException : HumanPresentableException
{
    public TravelCostNotInSingleCurrencyException() 
        : base("Koszt podróży jak i jego składowe muszą być podane w jednej walucie!", ExceptionCategory.ValidationError)
    {
        
    }
}