using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReStack.Common.Factories
{
    public static class ProcessFactory
    {
        public static Process CreateDefault(string fileName, string arguments = null)
        {
            var process = CreateDefault();
            process.StartInfo.FileName = fileName;
            process.StartInfo.Arguments = arguments;
            return process;
        }

        public static Process CreateDefault()
        {
            var process = new Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;
            return process;
        }
    }
}
