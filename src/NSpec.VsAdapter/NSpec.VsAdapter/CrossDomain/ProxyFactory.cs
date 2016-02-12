using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.CrossDomain
{
    public class ProxyFactory<TResult> : IProxyFactory<TResult>
    {
        public MarshalingProxy<TResult> CreateProxy(ITargetAppDomain targetDomain)
        {
            var marshaledType = typeof(MarshalingProxy<TResult>);

            var marshaledTypeName = marshaledType.FullName;

            var marshaledAssemblyName = marshaledType.Assembly.GetName().Name;

            var crossDomainProxy = (MarshalingProxy<TResult>)
                targetDomain.CreateInstanceAndUnwrap(marshaledAssemblyName, marshaledTypeName);

            return crossDomainProxy;
        }
    }
}
