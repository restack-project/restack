using ReStack.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReStack.Common.Interfaces
{
    public interface ILibrarySync
    {
        Task<ComponentLibrary> Sync(string source);
    }
}
