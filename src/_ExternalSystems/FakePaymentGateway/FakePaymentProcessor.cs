using System.Collections.Concurrent;
using FakePaymentGateway.DTO;

namespace FakePaymentGateway;

public class FakePaymentProcessor: IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private static readonly ConcurrentQueue<SetUpPaymentWithMetadata> _paymentsToAutomaticallyPay = new();
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    private readonly ILogger<FakePaymentProcessor> _logger;
    private Timer _timer;

    public FakePaymentProcessor(IServiceProvider serviceProvider, ILogger<FakePaymentProcessor> logger)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
    }

    private async void DoWork(object? state)
    {
        try
        {
            await _semaphore.WaitAsync();
            var dispatchTasks = new List<Task>();
            while (_paymentsToAutomaticallyPay.TryDequeue(out var payment))
            {
                dispatchTasks.Add(AutoPay(payment));
            }
            await Task.WhenAll(dispatchTasks);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }

    public void SetUpPayment(SetUpPaymentWithMetadata setupPayment, CancellationToken cancellationToken = default)
    {
        _paymentsToAutomaticallyPay.Enqueue(setupPayment);
    }

    public async Task AutoPay(SetUpPaymentWithMetadata paymentToAutoPay)
    {
        var httpClientFactory = _serviceProvider.GetService<IHttpClientFactory>();
        var httpClient = httpClientFactory.CreateClient("GroupFlights.Finance");

        var result = await httpClient.PostAsJsonAsync(paymentToAutoPay.WebhookUrl,
            new PaymentWebhookDto(paymentToAutoPay.PaymentId, paymentToAutoPay.Secret),
            CancellationToken.None);

        try
        {
            result.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
        }
    }

}