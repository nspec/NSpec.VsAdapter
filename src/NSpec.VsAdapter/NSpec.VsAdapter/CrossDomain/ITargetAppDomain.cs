using System;

namespace NSpec.VsAdapter.CrossDomain
{
    public interface ITargetAppDomain : IDisposable
    {
        Object CreateInstanceAndUnwrap(string marshalingAssemblyName, string marshalingTypeName);
    }
}
