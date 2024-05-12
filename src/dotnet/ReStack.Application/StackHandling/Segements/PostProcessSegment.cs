using ReStack.Common.Interfaces;
using ReStack.Common.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReStack.Application.StackHandling.Segements
{
    public class PostProcessSegment : BaseSegment
    {
        public PostProcessSegment(INotificationPublisher publisher, ILogRepository logRepository) : base(publisher, logRepository)
        {
        }

        public override Task<PipelineContext> Run(PipelineContext context, CancellationToken cancellationToken)
        {
            if (Directory.Exists(context.WorkDirectory))
                Directory.Delete(context.WorkDirectory, recursive: true);

            return Task.FromResult(context);
        }
    }
}
