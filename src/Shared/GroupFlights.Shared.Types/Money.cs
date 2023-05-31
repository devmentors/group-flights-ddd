namespace GroupFlights.Shared.Types;

public record Money(decimal Amount, Currency Currency)
{
    public static Money operator *(Money money, uint times)
    {
        return money with { Amount = money.Amount * times };
    }

    public static Money operator +(Money left, Money right)
    {
        if (left.Currency != right.Currency)
        {
            throw new MoneyOperationsArePossibleOnlyForSameCurrency();
        }

        return left with { Amount = left.Amount + right.Amount };
    }
}

public enum Currency
{
    PLN = 1,
    EUR = 2,
    USD = 3
}

public class MoneyOperationsArePossibleOnlyForSameCurrency : Exception
{
    public MoneyOperationsArePossibleOnlyForSameCurrency()
    {
        
    }
}