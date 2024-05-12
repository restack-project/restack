using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ReStack.Common.Interfaces;
using ReStack.Domain.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ReStack.Application.StackHandling
{
    public sealed class JobQueueHostedService : BackgroundService
    {
        private readonly IJobQueue _taskQueue;
        private readonly IServiceProvider _serviceProvider;
        private readonly ITokenCache _tokenCache;
        private readonly ILogger<JobQueueHostedService> _logger;
        private readonly SemaphoreSlim _semaphore;

        public JobQueueHostedService(
            IJobQueue taskQueue,
            IServiceProvider serviceProvider,
            ITokenCache tokenCache,
            IOptions<ApiSettings> options,
            ILogger<JobQueueHostedService> logger)
        {
            var maxWorkers = options.Value.JobWorkers == 0 ? 1 : options.Value.JobWorkers;

            _logger = logger;
            _taskQueue = taskQueue;
            _serviceProvider = serviceProvider;
            _tokenCache = tokenCache;
            _semaphore = new SemaphoreSlim(maxWorkers, maxWorkers);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{nameof(JobQueueHostedService)} is starting.");

            return ProcessTaskQueueAsync(stoppingToken);
        }

        private async Task ProcessTaskQueueAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var request = await _taskQueue.DequeueAsync(stoppingToken);

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                    Task.Run(() => ExecuteStack(request, stoppingToken), stoppingToken).ConfigureAwait(false);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

                }
                catch (OperationCanceledException)
                {
                    // Prevent throwing if stoppingToken was signaled
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred executing task work item.");
                }
            }
        }

        private async Task ExecuteStack(JobRequest request, CancellationToken stoppingToken)
        {
            try
            {
                await _semaphore.WaitAsync(stoppingToken);

                var token = _tokenCache.Add(request.JobId);

                var stackExecutor = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IStackExecutor>();
                await stackExecutor.Execute(request.StackId, request.JobId, token.Token);
            }
            catch (OperationCanceledException)
            {
                // Prevent throwing if stoppingToken was signaled
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred executing task work item.");
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{nameof(JobQueueHostedService)} is stopping.");

            await base.StopAsync(stoppingToken);
        }
    }
}
