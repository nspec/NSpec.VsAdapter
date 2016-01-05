using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.NSpecModding
{
    public interface ICrossDomainRunner<TInvocation, TResult>
    {
        TResult Run(
            string assemblyPath, 
            TInvocation invocation,
            Func<TInvocation, TResult> targetOperation);
    }
}
