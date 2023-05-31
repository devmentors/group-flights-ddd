namespace GroupFlights.Sales.Application.PaymentRegistry;

internal interface IPaymentRegistry
{
    Task SaveMapping(PaymentRegistryEntry paymentRegistryEntry, CancellationToken cancellationToken = default);
    Task<PaymentRegistryEntry> GetByPaymentId(Guid paymentId, CancellationToken cancellationToken = default);
}