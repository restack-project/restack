using ReStack.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReStack.Common.Interfaces
{
    public interface ILibraryComposer
    {
        Task<ComposeResult> Compose(string source);
    }

    public class ComposeResult
    {
        public ComponentLibrary ComponentLibrary { get; set; }
        public string GitDirectory { get; set; }
        public Dictionary<string, List<string>> Validations { get; private set; } = new();
        public bool Success { get => !Validations.Any(); }

        public void AddValidation(string key, string value)
        {
            if (!Validations.ContainsKey(key))
                Validations.Add(key, new List<string>());

            Validations[key].Add(value);
        }
    }
}
