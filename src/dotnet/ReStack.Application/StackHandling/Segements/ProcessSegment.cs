using Microsoft.Extensions.DependencyInjection;
using ReStack.Common.Factories;
using ReStack.Common.Interfaces;
using ReStack.Common.Interfaces.Repositories;
using ReStack.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ReStack.Application.StackHandling.Segements
{
    public class ProcessSegment : BaseSegment
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly List<Task> _jobAddedTasks;
        private readonly SemaphoreSlim _semaphore = new(1, 1);

        public ProcessSegment(
            INotificationPublisher publisher
            , ILogRepository logRepository
            , IServiceProvider serviceProvider
        ) : base(publisher, logRepository)
        {
            _jobAddedTasks = new List<Task>();
            _serviceProvider = serviceProvider;
        }

        public override async Task<PipelineContext> Run(PipelineContext context, CancellationToken cancellationToken)
        {
            var process = ProcessFactory.CreateDefault();

            try
            {
                context.Strategy.ConfigureProcess(process, context);

                process.StartInfo.WorkingDirectory = context.WorkDirectory;
                process.OutputDataReceived += (sender, e) => _jobAddedTasks.Add(ParseJob(context.Job, e.Data, false, cancellationToken));
                process.ErrorDataReceived += (sender, e) => _jobAddedTasks.Add(ParseJob(context.Job, e.Data, context.Stack.FailOnStdError, cancellationToken));

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                await process.WaitForExitAsync(cancellationToken);

                await Task.WhenAll(_jobAddedTasks);
            }
            catch (OperationCanceledException)
            {
                process.Kill(entireProcessTree: true);
                throw;
            }

            return context;
        }

        private async Task ParseJob(Job job, string output, bool isError, CancellationToken token)
        {
            if (!token.IsCancellationRequested && !string.IsNullOrWhiteSpace(output))
            {
                var log = new Log
                {
                    Timestamp = DateTime.UtcNow,
                    JobId = job.Id,
                    Message = output,
                    Error = isError
                };

                job.Logs.Add(log);

                if (log is not null)
                {
                    await Publish(job, log, token);
                }
            }
        }

        private async Task Publish(Job job, Log log, CancellationToken token)
        {
            if (!token.IsCancellationRequested)
            {
                try
                {
                    var logRepository = _serviceProvider.GetRequiredService<ILogRepository>();
                    var publisher = _serviceProvider.GetRequiredService<INotificationPublisher>();
                    log.JobId = job.Id;

                    await _semaphore.WaitAsync(token);

                    if (!token.IsCancellationRequested)
                    {
                        log = await logRepository.Add(log);
                        await publisher.LogAdded(log);
                    }
                }
                catch (OperationCanceledException)
                {
                    // Don't crash
                }
                finally
                {
                    _semaphore.Release();
                }
            }
        }
    }
}
