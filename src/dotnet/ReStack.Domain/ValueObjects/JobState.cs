using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReStack.Domain.ValueObjects
{
    public enum JobState
    {
        Queued, Running, Success, Failed, Cancelled
    }
}
