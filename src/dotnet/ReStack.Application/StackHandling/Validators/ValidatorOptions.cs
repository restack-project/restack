using Microsoft.Extensions.DependencyInjection;

namespace ReStack.Application.StackHandling.Validators
{
    public class ValidatorOptions
    {
        private readonly IServiceProvider _serviceProvider;

        public List<BaseStrategyValidator> Validators { get; private set; } = new();

        public ValidatorOptions(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ValidatorOptions Add<TE>() where TE : BaseStrategyValidator
        {
            var validator = (BaseStrategyValidator)_serviceProvider.GetRequiredService<TE>();
            Validators.Add(validator);
            return this;
        }
    }
}
