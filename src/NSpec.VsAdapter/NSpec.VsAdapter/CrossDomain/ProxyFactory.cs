using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// TODO check for deletion
#if false
namespace NSpec.VsAdapter.CrossDomain
{
    public class ProxyFactory<TResult> : IProxyFactory<TResult>
    {
        public Proxy<TResult> CreateProxy(ITargetAppDomain targetDomain)
        {
            var marshaledType = typeof(Proxy<TResult>);

            var marshaledTypeName = marshaledType.FullName;

            var marshaledAssemblyName = marshaledType.Assembly.GetName().Name;

            var crossDomainProxy = (Proxy<TResult>)
                targetDomain.CreateInstanceAndUnwrap(marshaledAssemblyName, marshaledTypeName);

            return crossDomainProxy;
        }
    }
}
#endif
