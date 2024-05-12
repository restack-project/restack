using ReStack.Application.StackHandling.Validators;
using ReStack.Domain.ValueObjects;
using System.Diagnostics;

namespace ReStack.Application.StackHandling.Languages
{
    public abstract class BaseLanguage
    {
        private readonly IServiceProvider _serviceProvider;

        public abstract ProgrammingLanguage Type { get; }
        public abstract void ConfigureProcess(Process process, PipelineContext context);

        public virtual void AddValidation(ValidatorOptions options) { }

        public BaseLanguage(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public List<BaseStrategyValidator> GetValidators()
        {
            var options = new ValidatorOptions(_serviceProvider);
            AddValidation(options);
            return options.Validators;
        }
    }
}
