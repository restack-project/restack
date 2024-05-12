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
    public class ValidateSegment : BaseSegment
    {
        public ValidateSegment(INotificationPublisher publisher, ILogRepository logRepository) : base(publisher, logRepository)
        {
        }

        public override async Task<PipelineContext> Run(PipelineContext context, CancellationToken cancellationToken)
        {
            var validators = context.Strategy.GetValidators();

            foreach (var validator in validators)
            {
                var logs = await validator.Validate(context.Stack);
                context.AddLog(logs);
            }

            return context;
        }
    }
}
