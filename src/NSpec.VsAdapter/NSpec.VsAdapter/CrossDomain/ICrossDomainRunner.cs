using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.CrossDomain
{
    public interface ICrossDomainRunner<TResult>
    {
        TResult Run(string assemblyPath, Func<TResult> targetOperation);
    }
}
