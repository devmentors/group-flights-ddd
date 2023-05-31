using GroupFlights.Finance.Core.Data;
using GroupFlights.Finance.Core.DTO;
using GroupFlights.Finance.Core.External;
using GroupFlights.Finance.Core.Models;
using GroupFlights.Finance.Core.Validators;
using GroupFlights.Finance.Shared.Events;
using GroupFlights.Finance.Shared.SetupPayment;
using GroupFlights.Shared.Plumbing.Events;
using GroupFlights.Shared.Types.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace GroupFlights.Finance.Core.Services;

internal class PaymentService : IPaymentService
{
    private readonly IPaymentGatewayFacade _paymentGatewayFacade;
    private readonly IPayerService _payerService;
    private readonly IEventDispatcher _eventDispatcher;
    private readonly PaymentValidator _paymentValidator;
    private readonly FinanceDbContext _dbContext;

    public PaymentService(IPaymentGatewayFacade paymentGatewayFacade,
        IPayerService payerService,
        IEventDispatcher eventDispatcher,
        PaymentValidator paymentValidator,
        FinanceDbContext dbContext)
    {
        _paymentGatewayFacade = paymentGatewayFacade ?? throw new ArgumentNullException(nameof(paymentGatewayFacade));
        _payerService = payerService ?? throw new ArgumentNullException(nameof(payerService));
        _eventDispatcher = eventDispatcher ?? throw new ArgumentNullException(nameof(eventDispatcher));
        _paymentValidator = paymentValidator ?? throw new ArgumentNullException(nameof(paymentValidator));
        _dbContext = dbContext;
    }
    
    public async Task SetupPayment(SetupPaymentDto paymentSetup, CancellationToken cancellationToken)
    {
        _paymentValidator.Validate(paymentSetup);

        var existingPayment = await _dbContext.Payments
            .SingleOrDefaultAsync(p => p.PaymentId.Equals(paymentSetup.PaymentId), cancellationToken);
        
        if (existingPayment is not null)
        {
            throw new AlreadyExistsException();
        }

        var payer = await _payerService.GetPayerById(paymentSetup.PayerId, cancellationToken);

        if (payer is null)
        {
            throw new DoesNotExistException(nameof(payer));
        }

        var secret = $"{Guid.NewGuid()}-{Guid.NewGuid()}";
        
        await _dbContext.AddAsync(new Payment(
            paymentSetup.PaymentId,
            paymentSetup.PayerId,
            paymentSetup.Amount,
            paymentSetup.DueDate,
            secret,
            false
        ), cancellationToken);

        await _paymentGatewayFacade.SetUpPayment(paymentSetup, payer, secret, cancellationToken);
        
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task OnPaymentPayed(PaymentWebhookDto webhookDto, CancellationToken cancellationToken = default)
    {
        var payment = await _dbContext.Payments
            .SingleOrDefaultAsync(p => p.PaymentId.Equals(webhookDto.PaymentId), cancellationToken);

        if (payment is null)
        {
            throw new DoesNotExistException();
        }

        if (webhookDto.PaymentGatewaySecret != payment.PaymentGatewaySecret)
        {
            throw new InvalidOperationException($"Payment secret mismatch for paymentId: {payment.PaymentId}");
        }

        var updatedPayment = payment with { Payed = true };
        _dbContext.Entry(payment).CurrentValues.SetValues(updatedPayment);

        await _eventDispatcher.PublishAsync(new PaymentCompleted(payment.PaymentId), cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}