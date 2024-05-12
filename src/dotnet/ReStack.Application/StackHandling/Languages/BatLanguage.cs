using Microsoft.Extensions.Options;
using ReStack.Application.StackHandling.Validators;
using ReStack.Domain.Settings;
using ReStack.Domain.Entities;
using ReStack.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReStack.Common.Interfaces;
using System.Collections;

namespace ReStack.Application.StackHandling.Languages
{
    public class BatLanguage : BaseLanguage
    {
        public override ProgrammingLanguage Type => ProgrammingLanguage.Bat;
        
        public BatLanguage(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override void AddValidation(ValidatorOptions options)
        {
            options.Add<IsWindowsValidator>();
        }

        public override void ConfigureProcess(Process process, PipelineContext context)
        {
            process.StartInfo.FileName = $"{Path.Combine(context.WorkDirectory, context.Stack.GetFileName())}";
        }
    }
}
