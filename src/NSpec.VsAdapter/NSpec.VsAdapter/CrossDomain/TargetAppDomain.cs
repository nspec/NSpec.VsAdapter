using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NSpec.VsAdapter.CrossDomain
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
