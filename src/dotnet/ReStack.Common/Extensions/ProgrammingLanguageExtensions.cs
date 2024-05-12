using ReStack.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReStack.Common.Extensions
{
    public static class ProgrammingLanguageExtensions
    {
        public static string GetExtension(this ProgrammingLanguage language)
        {
            return language switch
            {
                ProgrammingLanguage.Bat => ".bat",
                ProgrammingLanguage.Shell => ".sh",
                ProgrammingLanguage.Python => ".py",
                ProgrammingLanguage.PowerShell => ".ps1",
                _ => throw new ArgumentException($"{language.ToString()} has no extension configured, please add one."),
            };
        }
    }
}
