using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.NSpecModding
{
    public interface IMarshalingFactory<TResult>
    {
        MarshalingProxy<TResult> CreateProxy(ITargetAppDomain targetDomain);
    }
}
