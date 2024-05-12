using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ReStack.Application.StackHandling.Segements;
using ReStack.Application.StackHandling.Languages;
using ReStack.Common.Interfaces;
using ReStack.Domain.Entities;
using ReStack.Domain.Settings;
using ReStack.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReStack.Common.Interfaces.Repositories;

namespace ReStack.Application.StackHandling
{
    public class StackPipeline : IStackExecutor
    {
        private readonly List<BaseSegment> _segments = new();
        private readonly List<BaseSegment> _postSegments = new();

        private readonly IServiceProvider _serviceProvider;
        private readonly IStackRepository _stackRepository;
        private readonly IJobRepository _jobRepository;
        private readonly INotificationPublisher _notificationPublisher;
        private readonly IJobQueue _taskQueue;
        private readonly ApiSettings _settings;

        public StackPipeline(
            IServiceProvider serviceProvider,
            IStackRepository stackRepository,
            IJobRepository jobRepository,
            INotificationPublisher notificationPublisher,
            IJobQueue taskQueue,
            IOptions<ApiSettings> options
        )
        {
            _serviceProvider = serviceProvider;
            _stackRepository = stackRepository;
            _jobRepository = jobRepository;
            _notificationPublisher = notificationPublisher;
            _taskQueue = taskQueue;
            _settings = options.Value;

            RegisterSegment<ValidateSegment>();
            RegisterSegment<PrepareWorkDirectorySegment>();
            RegisterSegment<ProcessSegment>();
            RegisterSegment<PostProcessSegment>(_postSegments);
        }

        public async Task<Job> Queue(Stack stack)
        {
            var job = await AddAndPublishJob(stack);
            await _taskQueue.QueueBackgroundWorkItemAsync(new(stack.Id, job.Id));
            return job;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2016:Forward the 'CancellationToken' parameter to methods", Justification = "<Pending>")]
        public async Task<Job> Execute(int stackId, int jobId, CancellationToken cancellationToken)
        {
            var context = await CreateContext(stackId, jobId, cancellationToken);

            context.Job.Started = DateTime.UtcNow;
            await UpdateAndPublishJob(context.Job, JobState.Running);

            try
            {
                foreach (var segment in _segments)
                {
                    context = await segment.RunSegment(context, cancellationToken);
                    context.Job.State = context.Job.Logs.Any(x => x.Error) ? JobState.Failed : JobState.Success;
                    if (context.Job.State == JobState.Failed)
                        break;
                }

            }
            catch (OperationCanceledException)
            {
                context.AddLog(Log.CreateError("Job was canceled"));
                context.Job.State = JobState.Cancelled;
            }
            catch (Exception ex)
            {
                context.AddLog(Log.CreateError(ex.Message));
            }
            finally
            {
                foreach (var segment in _postSegments)
                {
                    context = await segment.RunSegment(context, cancellationToken);
                }
            }

            context.Job.Ended = DateTime.UtcNow;
            if (context.Job.State != JobState.Cancelled)
                context.Job.State = context.Job.Logs.Any(x => x.Error) ? JobState.Failed : JobState.Success;

            await UpdateAndPublishJob(context.Job);
            await _stackRepository.CalculateStats(context.Stack.Id);
            await _notificationPublisher.StackChanged(context.Stack);

            return context.Job;
        }

        private async Task<PipelineContext> CreateContext(int stackId, int jobId, CancellationToken cancellationToken)
        {
            var stack = await _stackRepository.Get(stackId, cancellationToken);
            var job = await _jobRepository.Get(jobId, cancellationToken);
            var workDirectory = Path.Combine(_settings.ExecuteStorage, Guid.NewGuid().ToString());
            var strategy = GetStrategy(stack.Type);

            return new PipelineContext(stack, job, workDirectory, strategy);
        }

        private async Task<Job> UpdateAndPublishJob(Job job, JobState? state = null)
        {
            if (state.HasValue)
                job.State = state.Value;

            job = await _jobRepository.Update(job);
            await _notificationPublisher.JobChanged(job);

            return job;
        }

        private async Task<Job> AddAndPublishJob(Stack stack)
        {
            var job = new Job()
            {
                Stack = stack,
                StackId = stack.Id,
                State = JobState.Queued,
                Started = DateTime.UtcNow,
                Sequence = await _jobRepository.NextSequence(stack.Id)
            };
            job = await _jobRepository.Add(job);
            await _notificationPublisher.JobChanged(job);
            return job;
        }

        private StackPipeline RegisterSegment<TE>(List<BaseSegment> segments = null) where TE : BaseSegment
        {
            var segment = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<TE>();
            segments ??= _segments;
            segments.Add(segment);
            return this;
        }

        private BaseLanguage GetStrategy(ProgrammingLanguage type)
        {
            var strategies = _serviceProvider.GetServices<BaseLanguage>();
            var strategy = strategies.FirstOrDefault(x => x.Type == type);

            return strategy ?? throw new Exception($"No strategy found to handle this type of stack '{type}'");
        }
    }
}
