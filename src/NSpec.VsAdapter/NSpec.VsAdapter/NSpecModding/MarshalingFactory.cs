using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.NSpecModding
{
    public class MarshalingFactory<TInvocation, TResult> : IMarshalingFactory<TInvocation, TResult>
    {
        public MarshalingProxy<TInvocation, TResult> CreateProxy(ITargetAppDomain targetDomain)
        {
            var marshaledType = typeof(MarshalingProxy<TInvocation, TResult>);

            var marshaledTypeName = marshaledType.FullName;

            var marshaledAssemblyName = marshaledType.Assembly.GetName().Name;

            var crossDomainProxy = (MarshalingProxy<TInvocation, TResult>)
                targetDomain.CreateInstanceAndUnwrap(marshaledAssemblyName, marshaledTypeName);

            return crossDomainProxy;
        }
    }
}
