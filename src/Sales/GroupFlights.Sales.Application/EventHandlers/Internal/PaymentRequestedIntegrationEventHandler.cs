using GroupFlights.Finance.Shared;
using GroupFlights.Finance.Shared.SetupPayment;
using GroupFlights.Sales.Application.PaymentRegistry;
using GroupFlights.Sales.Shared.IntegrationEvents;
using GroupFlights.Shared.Plumbing.Events;

namespace GroupFlights.Sales.Application.EventHandlers.Internal;

internal class PaymentRequestedIntegrationEventHandler : IEventHandler<PaymentRequestedIntegrationEvent>
{
    private readonly IPaymentRegistry _paymentRegistry;
    private readonly IFinanceApi _financeApi;

    public PaymentRequestedIntegrationEventHandler(IPaymentRegistry paymentRegistry, IFinanceApi financeApi)
    {
        _paymentRegistry = paymentRegistry ?? throw new ArgumentNullException(nameof(paymentRegistry));
        _financeApi = financeApi ?? throw new ArgumentNullException(nameof(financeApi));
    }
    
    public async Task HandleAsync(PaymentRequestedIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        await _paymentRegistry.SaveMapping(
            new PaymentRegistryEntry(@event.PaymentId, @event.Source.SourceType, @event.Source.SourceId),
            cancellationToken);
        
        await _financeApi.SetupPayment(new SetupPaymentDto(
                @event.PaymentId,
                @event.PayerId,
                @event.Amount,
                @event.DueDate),
            cancellationToken);
    }
}