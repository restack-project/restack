using ReStack.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReStack.Common.Exceptions
{
    public class ApiException : Exception
    {
        public ErrorModelType Type { get; set; }

        public ApiException(ErrorModelType type) : base(type.ToString())
        {
            Type = type;
        }
    }
}
