using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// TODO check for deletion
#if false
namespace NSpec.VsAdapter.CrossDomain
{
    public interface ICrossDomainRunner<TResult>
    {
        TResult Run(string binaryPath, Func<TResult> targetOperation);
    }
}
#endif
