using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReStack.Common.Interfaces
{
    public interface IJobQueue
    {
        ValueTask QueueBackgroundWorkItemAsync(JobRequest request);
        ValueTask<JobRequest> DequeueAsync(CancellationToken cancellationToken);
    }

    public record JobRequest
    {
        public int StackId { get; private set; }
        public int JobId { get; private set; }

        public JobRequest(int stackId, int jobId)
        {
            StackId = stackId;
            JobId = jobId;
        }
    }
}
