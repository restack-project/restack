using ReStack.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReStack.Application.StackHandling
{
    public class TokenCache : ITokenCache
    {
        private readonly Dictionary<int, CancellationTokenSource> _tokens = new();

        public void Delete(int jobId)
        {
            _tokens.Remove(jobId);
        }

        public CancellationTokenSource Add(int jobId)
        {
            if (_tokens.ContainsKey(jobId))
                _tokens[jobId].Cancel();

            _tokens.Add(jobId, new CancellationTokenSource());
            
            return _tokens[jobId];
        }

        public CancellationTokenSource Get(int jobId)
        {
            return _tokens.ContainsKey(jobId) ? _tokens[jobId] : default;
        }
    }
}
