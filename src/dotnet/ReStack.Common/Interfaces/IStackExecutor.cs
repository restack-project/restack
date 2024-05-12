using ReStack.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReStack.Common.Interfaces
{
    public interface IStackExecutor
    {
        Task<Job> Queue(Stack stack);
        Task<Job> Execute(int stackId, int jobId, CancellationToken cancellationToken = default);
    }
}
