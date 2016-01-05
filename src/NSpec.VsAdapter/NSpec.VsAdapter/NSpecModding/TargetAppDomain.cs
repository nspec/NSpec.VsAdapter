using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSpec.VsAdapter.NSpecModding
{
    public class TargetAppDomain : ITargetAppDomain
    {
        public TargetAppDomain(AppDomain appDomain)
        {
            this.appDomain = appDomain;
        }

        public object CreateInstanceAndUnwrap(string marshalingAssemblyName, string marshalingTypeName)
        {
            return appDomain.CreateInstanceAndUnwrap(marshalingAssemblyName, marshalingTypeName);
        }

        public void Unload()
        {
            AppDomain.Unload(appDomain);
        }

        readonly AppDomain appDomain;
    }
}
