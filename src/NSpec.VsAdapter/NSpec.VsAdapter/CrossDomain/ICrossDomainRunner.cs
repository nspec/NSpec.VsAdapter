using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.CrossDomain
{
    public interface ICrossDomainRunner<TProxyable, TResult>
        where TProxyable : IDisposable
    {
        TResult Run(string binaryPath,
            Func<TProxyable, TResult> operation,
            Func<Exception, string, TResult> fail);
    }
}
