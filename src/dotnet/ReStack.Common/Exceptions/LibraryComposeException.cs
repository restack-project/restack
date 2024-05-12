using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReStack.Common.Exceptions
{
    // TODO use model validation exception
    public class LibraryComposeException : Exception
    {
        public Dictionary<string, List<string>> Validations { get; }

        public LibraryComposeException(Dictionary<string, List<string>> validations)
        {
            Validations = validations;
        }
    }
}
