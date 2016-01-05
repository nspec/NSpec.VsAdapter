using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.NSpecModding
{
    public interface IMarshalingFactory<TInvocation, TResult>
    {
        MarshalingWrapper<TInvocation, TResult> CreateWrapper(AppDomain targetDomain);
    }
}
