using System;
using System.Reflection;

namespace NSpec.VsAdapter.Core.CrossDomain
{
    public class TargetAppDomain : ITargetAppDomain
    {
        public TargetAppDomain(AppDomain appDomain)
        {
            this.appDomain = appDomain;
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
            AppDomain.Unload(appDomain);
        }

        readonly AppDomain appDomain;
    }
}
