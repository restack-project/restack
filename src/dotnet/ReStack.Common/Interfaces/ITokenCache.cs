using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReStack.Common.Interfaces
{
    public interface ITokenCache
    {
        CancellationTokenSource Add(int jobId);
        CancellationTokenSource Get(int jobId);
        void Delete(int jobId);
    }
}
