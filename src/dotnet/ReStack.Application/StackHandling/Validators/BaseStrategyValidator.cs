using ReStack.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReStack.Application.StackHandling.Validators
{
    public abstract class BaseStrategyValidator
    {
        public List<Log> Logs { get; set; } = new();

        public abstract Task<List<Log>> Validate(Stack stack);
    }
}
