using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using ReStack.Common.Interfaces;

namespace ReStack.Application.StackHandling
{
    public sealed class JobQueue : IJobQueue
    {
        private readonly Channel<JobRequest> _queue;

        public JobQueue(int capacity)
        {
            BoundedChannelOptions options = new(capacity)
            {
                FullMode = BoundedChannelFullMode.Wait
            };
            _queue = Channel.CreateBounded<JobRequest>(options);
        }

        public async ValueTask QueueBackgroundWorkItemAsync(JobRequest request)
        {
            await _queue.Writer.WriteAsync(request);
        }

        public async ValueTask<JobRequest> DequeueAsync(CancellationToken cancellationToken)
        {
            var workItem = await _queue.Reader.ReadAsync(cancellationToken);
            return workItem;
        }
    }
}
