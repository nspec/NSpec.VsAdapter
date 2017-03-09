using System;

namespace NSpec.VsAdapter.Core.CrossDomain
{
    public interface ITargetAppDomain : IDisposable
    {
        Object CreateInstanceAndUnwrap(string marshalingAssemblyName, string marshalingTypeName);
    }
}
