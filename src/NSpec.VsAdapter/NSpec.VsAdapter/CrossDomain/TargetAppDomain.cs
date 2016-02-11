using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NSpec.VsAdapter.CrossDomain
{
    public class TargetAppDomain : ITargetAppDomain
    {
        public TargetAppDomain(AppDomain appDomain, string binaryPath)
        {
            this.appDomain = appDomain;

            var resolver = new AssemblyResolver(binaryPath);

            resolveHandler = resolver.Failed;

            this.appDomain.AssemblyResolve += resolveHandler;
        }

        public object CreateInstanceAndUnwrap(string marshalingAssemblyName, string marshalingTypeName)
        {
            return appDomain.CreateInstanceAndUnwrap(
                assemblyName: marshalingAssemblyName, 
                typeName: marshalingTypeName,
                ignoreCase: false,
                bindingAttr: BindingFlags.Default,
                binder: null,
                args: null,
                culture: null,
                activationAttributes: null);
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
