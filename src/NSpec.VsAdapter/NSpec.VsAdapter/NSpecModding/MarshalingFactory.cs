using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.NSpecModding
{
    public class MarshalingFactory<TInvocation, TResult> : IMarshalingFactory<TInvocation, TResult>
    {
        public MarshalingWrapper<TInvocation, TResult> CreateWrapper(ITargetAppDomain targetDomain)
        {
            var marshaledType = typeof(MarshalingWrapper<TInvocation, TResult>);

            var marshaledTypeName = marshaledType.FullName;

            var marshaledAssemblyName = marshaledType.Assembly.GetName().Name;

            var marshalingWrapper = (MarshalingWrapper<TInvocation, TResult>)
                targetDomain.CreateInstanceAndUnwrap(marshaledAssemblyName, marshaledTypeName);

            return marshalingWrapper;
        }
    }
}
