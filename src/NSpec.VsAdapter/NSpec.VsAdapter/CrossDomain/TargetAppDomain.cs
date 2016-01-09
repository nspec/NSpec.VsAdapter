using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSpec.VsAdapter.CrossDomain
{
    public class TargetAppDomain : ITargetAppDomain
    {
        public TargetAppDomain(AppDomain appDomain, ResolveEventHandler resolveHandler)
        {
            this.appDomain = appDomain;
            this.resolveHandler = resolveHandler;
        }

        public object CreateInstanceAndUnwrap(string marshalingAssemblyName, string marshalingTypeName)
        {
            return appDomain.CreateInstanceAndUnwrap(marshalingAssemblyName, marshalingTypeName);
        }

        public void Dispose()
        {
            appDomain.AssemblyResolve -= resolveHandler;

            AppDomain.Unload(appDomain);
        }

        readonly AppDomain appDomain;
        readonly ResolveEventHandler resolveHandler;
    }
}
