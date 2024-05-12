using Microsoft.Extensions.Options;
using ReStack.Domain.Settings;
using ReStack.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReStack.Application.StackHandling.Languages
{
    public class PythonLanguage : BaseLanguage
    {
        private ApiSettings _settings;

        public override ProgrammingLanguage Type => ProgrammingLanguage.Python;

        public PythonLanguage(IServiceProvider serviceProvider, IOptions<ApiSettings> options) : base(serviceProvider)
        {
            _settings = options.Value;
        }

        public override void ConfigureProcess(Process process, PipelineContext context)
        {
            process.StartInfo.FileName = _settings.UsePythonV3 ? "python3" : "python";
            process.StartInfo.Arguments = $"{Path.Combine(context.WorkDirectory, context.Stack.GetFileName())}";
        }
    }
}
