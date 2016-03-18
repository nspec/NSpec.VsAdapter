using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Core.CrossDomain
{
    public interface ITargetAppDomain : IDisposable
    {
        Object CreateInstanceAndUnwrap(string marshalingAssemblyName, string marshalingTypeName);
    }
}
