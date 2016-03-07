using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.CrossDomain
{
    public class ProxyableFactory<T> : IProxyableFactory<T> where T : Proxyable
    {
        public T CreateProxy(ITargetAppDomain targetDomain)
        {
            var marshaledType = typeof(T);

            var marshaledTypeName = marshaledType.FullName;

            var marshaledAssemblyName = marshaledType.Assembly.FullName;

            var crossDomainProxy = (T)targetDomain.CreateInstanceAndUnwrap(marshaledAssemblyName, marshaledTypeName);

            return crossDomainProxy;
        }
    }
}
