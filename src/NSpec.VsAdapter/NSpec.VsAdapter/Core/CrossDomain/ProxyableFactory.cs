using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Core.CrossDomain
{
    public class ProxyableFactory<TImplementation, TInterface> : IProxyableFactory<TInterface>
        where TImplementation : Proxyable, TInterface
    {
        public TInterface CreateProxy(ITargetAppDomain targetDomain)
        {
            var marshaledType = typeof(TImplementation);

            var marshaledTypeName = marshaledType.FullName;

            var marshaledAssemblyName = marshaledType.Assembly.FullName;

            var crossDomainProxy = (TImplementation)targetDomain.CreateInstanceAndUnwrap(
                marshaledAssemblyName, marshaledTypeName);

            return crossDomainProxy;
        }
    }
}
