using System.Data;
using GroupFlights.Shared.Types.Exceptions;

namespace GroupFlights.Inquiries.Core.Models;

internal record Inquiry(
    InquiryId Id,
    Inquirer Inquirer,
    InquiredFlight Travel,
    InquiredFlight Return,
    PassengersData DeclaredPassengers,
    IReadOnlyCollection<PriorityChoice> Priorities,
    bool CheckedBaggageRequired,
    bool AdditionalServicesRequired,
    string Comments)
{
    private Inquiry()
        : this(default, default, default, default, default, 
            default, default, default, default)
    {
    }

    public InquiryVerificationResult? VerificationResult { get; private set; }
    public string RejectionReason { get; private set; }
    public Guid? OfferId { get; private set; }

    public void Accept(Guid? offerToCreateId)
    {
        EnsureNotAlreadyVerified();
        OfferId = offerToCreateId;
        VerificationResult = InquiryVerificationResult.Accepted;
    }
    
    public void Reject(string rejectionReason)
    {
        EnsureNotAlreadyVerified();
        VerificationResult = InquiryVerificationResult.Rejected;
        RejectionReason = rejectionReason;
    }
    
    private void EnsureNotAlreadyVerified()
    {
        if (VerificationResult is not null)
        {
            throw new InquiryAlreadyVerifiedException();
        }
    }
}

internal record InquiryId(Guid Value);

public enum InquiryVerificationResult
{
    Accepted = 1,
    Rejected = 2
}

internal class InquiryAlreadyVerifiedException : HumanPresentableException
{
    public InquiryAlreadyVerifiedException() : base("Zapytanie było już zweryfikowane!", ExceptionCategory.ValidationError)
    {
    }
}