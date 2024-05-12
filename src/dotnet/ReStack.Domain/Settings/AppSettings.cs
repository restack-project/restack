using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReStack.Domain.Settings
{
    public class ApiSettings
    {
        private string _storage;

        public string Storage { get => _storage; set => _storage = value; }
        public string StackStorage { get => Path.Combine(_storage, "stacks"); }
        public string ComponentStorage { get => Path.Combine(_storage, "components"); }
        public string KeysStorage { get => Path.Combine(_storage, "keys"); }
        public string ExecuteStorage { get => Path.Combine(_storage, "executing"); }

        public string SshKey_Default { get; set; }
        public int JobQueue { get; set; }
        public int JobWorkers { get; set; }
        public bool UsePythonV3 { get; set; }
    }
}
