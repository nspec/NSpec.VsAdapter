using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.CrossDomain
{
    public class ProxyableFactory<TProxy> : IProxyableFactory<TProxy> where TProxy : Proxyable
    {
        public TProxy CreateProxy(ITargetAppDomain targetDomain)
        {
            var marshaledType = typeof(TProxy);

            var marshaledTypeName = marshaledType.FullName;

            var marshaledAssemblyName = marshaledType.Assembly.FullName;

            var crossDomainProxy = (TProxy)targetDomain.CreateInstanceAndUnwrap(
                marshaledAssemblyName, marshaledTypeName);

            return crossDomainProxy;
        }
    }
}
