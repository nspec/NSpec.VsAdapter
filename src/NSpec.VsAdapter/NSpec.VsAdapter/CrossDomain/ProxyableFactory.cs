using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.CrossDomain
{
    public class ProxyableFactory<TProxyableImpl, TIProxyable> : IProxyableFactory<TIProxyable>
        where TProxyableImpl : Proxyable, TIProxyable
    {
        public TIProxyable CreateProxy(ITargetAppDomain targetDomain)
        {
            var marshaledType = typeof(TProxyableImpl);

            var marshaledTypeName = marshaledType.FullName;

            var marshaledAssemblyName = marshaledType.Assembly.FullName;

            var crossDomainProxy = (TProxyableImpl)targetDomain.CreateInstanceAndUnwrap(
                marshaledAssemblyName, marshaledTypeName);

            return crossDomainProxy;
        }
    }
}
