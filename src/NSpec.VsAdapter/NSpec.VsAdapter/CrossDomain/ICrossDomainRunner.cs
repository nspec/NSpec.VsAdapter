using System;

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
