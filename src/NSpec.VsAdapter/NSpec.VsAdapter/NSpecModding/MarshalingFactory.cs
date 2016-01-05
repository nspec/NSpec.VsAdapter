using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.NSpecModding
{
    public class MarshalingFactory<TInvocation, TResult> : IMarshalingFactory<TInvocation, TResult>
    {
        public MarshalingWrapper<TInvocation, TResult> CreateWrapper(AppDomain targetDomain)
        {
            var marshalingType = typeof(MarshalingWrapper<TInvocation, TResult>);

            var marshalingTypeName = marshalingType.FullName;

            var marshalingAssemblyName = marshalingType.Assembly.GetName().Name;

            var marshalingWrapper = (MarshalingWrapper<TInvocation, TResult>)
                targetDomain.CreateInstanceAndUnwrap(marshalingAssemblyName, marshalingTypeName);

            return marshalingWrapper;
        }
    }
}
