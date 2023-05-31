namespace FakePaymentGateway.DTO;

public record Money(decimal Amount, Currency Currency)
{
    public static Money operator *(Money money, uint times)
    {
        return money with { Amount = money.Amount * times };
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