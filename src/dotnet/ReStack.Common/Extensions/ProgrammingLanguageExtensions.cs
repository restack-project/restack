using ReStack.Domain.ValueObjects;

namespace ReStack.Common.Extensions;

public static class ProgrammingLanguageExtensions
{
    public static string GetExtension(this ProgrammingLanguage language)
    {
        return language switch
        {
            ProgrammingLanguage.Bat => ".bat",
            ProgrammingLanguage.Shell => ".sh",
            ProgrammingLanguage.PowerShell => ".ps1",
            _ => throw new ArgumentException($"{language.ToString()} has no extension configured, please add one."),
        };
    }
}
