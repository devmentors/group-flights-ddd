namespace GroupFlights.Sales.Application.PaymentRegistry;

internal class PaymentRegistry : IPaymentRegistry
{
    //TODO: Przepisac na wlasciwe persistence
    private static readonly Dictionary<Guid, PaymentRegistryEntry> _inMemoryRegistry = new ();
    
    public Task SaveMapping(PaymentRegistryEntry paymentRegistryEntry, CancellationToken cancellationToken = default)
    {
        _inMemoryRegistry.Add(paymentRegistryEntry.PaymentId, paymentRegistryEntry);
        return Task.CompletedTask;
    }

    public Task<PaymentRegistryEntry> GetByPaymentId(Guid paymentId, CancellationToken cancellationToken = default)
    {
        _inMemoryRegistry.TryGetValue(paymentId, out var payment);
        return Task.FromResult(payment);
    }
}