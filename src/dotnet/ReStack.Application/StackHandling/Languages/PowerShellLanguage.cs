using ReStack.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReStack.Application.StackHandling.Languages
{
    public class PowerShellLanguage : BaseLanguage
    {
        public override ProgrammingLanguage Type => ProgrammingLanguage.PowerShell;

        public PowerShellLanguage(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override void ConfigureProcess(Process process, PipelineContext context)
        {
            process.StartInfo.FileName = OperatingSystem.IsLinux() ? "pwsh" : "powershell";
            process.StartInfo.Arguments = $"{Path.Combine(context.WorkDirectory, context.Stack.GetFileName())}";
        }
    }
}
