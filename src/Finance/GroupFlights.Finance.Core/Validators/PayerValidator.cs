using GroupFlights.Finance.Core.DTO;
using GroupFlights.Finance.Core.Exceptions;
using GroupFlights.Finance.Core.Models;

namespace GroupFlights.Finance.Core.Validators;

internal class PayerValidator
{
    public void Validate(Payer payer)
    {
        if (payer is null)
        {
            throw new InvalidInputException(nameof(payer));
        }
        
        if (payer.UserId.Equals(Guid.Empty))
        {
            throw new InvalidInputException(nameof(payer.UserId));
        }

        if (string.IsNullOrEmpty(payer.PayerFullName))
        {
            throw new InvalidInputException(nameof(payer.PayerFullName)); 
        }
        
        if (string.IsNullOrEmpty(payer.PayerFullName))
        {
            throw new InvalidInputException(nameof(payer.PayerFullName)); 
        }
        
        if (string.IsNullOrEmpty(payer.TaxNumber) && payer.IsLegalEntity)
        {
            throw new InvalidInputException(nameof(payer.TaxNumber)); 
        }
        
        if (string.IsNullOrEmpty(payer.Address))
        {
            throw new InvalidInputException(nameof(payer.Address)); 
        }
    }
}