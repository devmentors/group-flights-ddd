using GroupFlights.TimeManagement.Core.Data.EF;
using GroupFlights.TimeManagement.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GroupFlights.TimeManagement.Core.Schedulers;

internal class DeadlineScheduler : IHostedService, IDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DeadlineScheduler> _logger;
    private Timer _timer;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public DeadlineScheduler(IServiceProvider serviceProvider, ILogger<DeadlineScheduler> logger)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var dbContextInitialized = false;
            while (dbContextInitialized is false)
            {
                try
                {
                    var dbContext = scope.ServiceProvider.GetService<TimeManagementDbContext>();
                    await dbContext.Deadlines.FirstOrDefaultAsync(cancellationToken);
                    dbContextInitialized = true;
                    break;
                }
                catch (Exception ex)
                {
                    await Task.Delay(2000);
                }
            }
        }

        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
    }

    private async void DoWork(object? state)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var deadlineService = scope.ServiceProvider.GetService<IDeadlineService>();
            await _semaphore.WaitAsync();
            await deadlineService.ProcessActiveDeadlines();
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
}