using GroupFlights.Finance.Shared;
using GroupFlights.Finance.Shared.SetupPayment;
using GroupFlights.Postsale.Shared.IntegrationEvents;
using GroupFlights.Sales.Shared.IntegrationEvents;
using GroupFlights.Shared.Plumbing.Events;

namespace GroupFlights.Postsale.Application.EventHandlers.Internal;

internal class PostsalePaymentRequestedIntegrationEventHandler : IEventHandler<PostsalePaymentRequestedIntegrationEvent>
{
    private readonly IFinanceApi _financeApi;

    public PostsalePaymentRequestedIntegrationEventHandler(IFinanceApi financeApi)
    {
        _financeApi = financeApi ?? throw new ArgumentNullException(nameof(financeApi));
    }
    
    public async Task HandleAsync(PostsalePaymentRequestedIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        await _financeApi.SetupPayment(new SetupPaymentDto(
                @event.PaymentId,
                @event.PayerId,
                @event.Amount,
                @event.DueDate),
            cancellationToken);
    }
}