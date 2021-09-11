using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class CheckForPingService : IHostedService
{
    readonly ILogger _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public CheckForPingService(ILogger<CheckForPingService> logger, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Staring");
        Task.Run(() => RunAsync(cancellationToken));
        return Task.CompletedTask;
    }

    async Task RunAsync(CancellationToken cancellationToken)
    {
        while(true)
        {
            _logger.LogInformation("Checking last ping");

            using(var scope = _scopeFactory.CreateScope())
            {
                var pingRecorder = scope.ServiceProvider.GetService<PingRecorder>();

                var lastPing = await pingRecorder.GetLastPingDateTime();

                _logger.LogInformation($"Last ping value: {lastPing}");

                if(lastPing != PingRecorder.DefaultLastPing && lastPing < DateTime.UtcNow.AddMinutes(5)) {
                    // Send notification
                    _logger.LogInformation("Need to send alert.");
                }
            }

            if(cancellationToken.IsCancellationRequested)
            {
                break;
            }

            await Task.Delay((int)TimeSpan.FromSeconds(10).TotalMilliseconds);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}