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
            CrossDomainRunner<TProxyable, TResult>.RemoteOperation operation, 
            CrossDomainRunner<TProxyable, TResult>.FailureCallback fail);
    }
}
