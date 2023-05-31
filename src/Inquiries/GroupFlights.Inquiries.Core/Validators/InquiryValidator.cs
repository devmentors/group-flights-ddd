using GroupFlights.Inquiries.Core.DTO;
using GroupFlights.Inquiries.Core.Exceptions;
using GroupFlights.Inquiries.Core.External;
using GroupFlights.Shared.Types;
using GroupFlights.Shared.Types.Time;

namespace GroupFlights.Inquiries.Core.Validators;

internal class InquiryValidator
{
    private readonly IIataAirportService _iataAirportService;
    private readonly IClock _clock;

    public InquiryValidator(IIataAirportService iataAirportService, IClock clock)
    {
        _iataAirportService = iataAirportService ?? throw new ArgumentNullException(nameof(iataAirportService));
        _clock = clock ?? throw new ArgumentNullException(nameof(clock));
    }
    
    public async Task Validate(InquiryInputDto inquiryInput)
    {
        ValidateInquirer(inquiryInput);
        ValidateDeclarePassengers(inquiryInput);
        
        await ValidateFlight(inquiryInput.Travel);
        await ValidateFlight(inquiryInput.Return);

        EnsureFirstFlightNotTooFarFromNow(inquiryInput);
        EnsureFirstFlightIsInFuture(inquiryInput);
    }

    private void ValidateInquirer(InquiryInputDto inquiryInputDto)
    {
        var inquirer = inquiryInputDto.Inquirer;

        if (inquirer is null)
        {
            throw new InvalidInputException(nameof(inquirer));
        }

        if (string.IsNullOrEmpty(inquirer.Name))
        {
            throw new InvalidInputException(nameof(inquirer.Name));
        }
        
        if (string.IsNullOrEmpty(inquirer.Surname))
        {
            throw new InvalidInputException(nameof(inquirer.Surname));
        }
        
        if (string.IsNullOrEmpty(inquirer.Email))
        {
            throw new InvalidInputException(nameof(inquirer.Email));
        }
        
        new Email(inquirer.Email);
        
        if (string.IsNullOrEmpty(inquirer.PhoneNumber))
        {
            throw new InvalidInputException(nameof(inquirer.PhoneNumber));
        }
    }

    private void ValidateDeclarePassengers(InquiryInputDto inquiryInputDto)
    {
        if (inquiryInputDto.DeclaredPassengers is null)
        {
            throw new InvalidInputException(nameof(inquiryInputDto.DeclaredPassengers));
        }
        
        var declaredPassengersCount = inquiryInputDto.DeclaredPassengers.AdultCount +
                                              inquiryInputDto.DeclaredPassengers.ChildrenCount +
                                              inquiryInputDto.DeclaredPassengers.InfantCount;
        var minimalPassengersCount = 10;
        if (declaredPassengersCount < minimalPassengersCount)
        {
            throw new ThisIsNotAGroupFlightException(minimalPassengersCount, declaredPassengersCount);
        }

        var maxPassengersCount = 1000;
        if (declaredPassengersCount > maxPassengersCount)
        {
            throw new MaxPassengersCountExceededException(1000, declaredPassengersCount);
        }
    }

    private async Task ValidateFlight(InquiryFlightDto flightDto)
    {
        if (flightDto is null)
        {
            throw new InvalidInputException(nameof(flightDto));
        }

        if (flightDto.Date == default)
        {
            throw new InvalidInputException(nameof(flightDto.Date));
        }
        
        if (flightDto.SourceAirport == default)
        {
            throw new InvalidInputException(nameof(flightDto.SourceAirport));
        }
        
        if (flightDto.TargetAirport == default)
        {
            throw new InvalidInputException(nameof(flightDto.TargetAirport));
        }

        var matchingSourceAirport = await _iataAirportService.GetAirportByCode(flightDto.SourceAirport.Code);

        if (matchingSourceAirport is null)
        {
            throw new InvalidAirportException(flightDto.SourceAirport.Code);
        }
        
        if (matchingSourceAirport is null)
        {
            throw new InvalidAirportException(flightDto.SourceAirport.Code);
        }
        
        var matchingTargetAirport = await _iataAirportService.GetAirportByCode(flightDto.TargetAirport.Code);

        if (matchingTargetAirport is null)
        {
            throw new InvalidAirportException(flightDto.TargetAirport.Code);
        }
        
        if (matchingTargetAirport is null)
        {
            throw new InvalidAirportException(flightDto.TargetAirport.Code);
        }
    }

    private void EnsureFirstFlightNotTooFarFromNow(InquiryInputDto inquiryInputDto)
    {
        var now = _clock.UtcNow;
        var maxMonthsAhead = 12;

        if (inquiryInputDto.Travel.Date - now >= TimeSpan.FromDays(12 * maxMonthsAhead))
        {
            throw new FirstFlightTooFarInFutureException(maxMonthsAhead, inquiryInputDto.Travel.Date);
        }
    }

    private void EnsureFirstFlightIsInFuture(InquiryInputDto inquiryInputDto)
    {
        var now = _clock.UtcNow;
        var minDaysAhead = 2;
        
        if (inquiryInputDto.Travel.Date - now < TimeSpan.FromDays(minDaysAhead))
        {
            throw new TooLateToOrganizeFlightException(minDaysAhead, inquiryInputDto.Travel.Date);
        }
    }
}