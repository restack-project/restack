using ReStack.Common.Interfaces;
using ReStack.Common.Interfaces.Repositories;
using ReStack.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReStack.Application.StackHandling.Segements
{
    public abstract class BaseSegment
    {
        private readonly INotificationPublisher _publisher;
        private readonly ILogRepository _logRepository;

        public abstract Task<PipelineContext> Run(PipelineContext context, CancellationToken cancellationToken);

        public BaseSegment(INotificationPublisher publisher, ILogRepository logRepository)
        {
            _publisher = publisher;
            _logRepository = logRepository;
        }

        public async Task<PipelineContext> RunSegment(PipelineContext context, CancellationToken cancellationToken)
        {
            if (!context.StopRequested || cancellationToken.IsCancellationRequested)
            {
                try
                {
                    context = await Run(context, cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    context.StopRequested = true;
                    throw;
                }
                catch (Exception ex)
                {
                    context.AddLog(Log.CreateError(ex.Message));
                }

                await PublishAddedLogs(context);
            }

            return context;
        }

        private async Task PublishAddedLogs(PipelineContext context)
        {
            foreach (var log in context.Job.Logs.Where(x => x.Id == 0).OrderBy(x => x.Timestamp))
            {
                var savedLog = await _logRepository.Add(log);
                await _publisher.LogAdded(savedLog);
            }
        }
    }
}
