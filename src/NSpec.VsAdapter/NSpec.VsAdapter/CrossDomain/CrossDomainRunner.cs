using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.CrossDomain
{
    // TODO rename to RemoteDomainRunner or TargetDomainRunner. Same for interface.

    public class CrossDomainRunner<TProxyable, TResult> : ICrossDomainRunner<TProxyable, TResult>
        where TProxyable : IDisposable
    {
        // initial implementation taken from 
        // http://thevalerios.net/matt/2008/06/run-anonymous-methods-in-another-appdomain/

        public CrossDomainRunner(IAppDomainFactory appDomainFactory, IProxyableFactory<TProxyable> proxyableFactory)
        {
            this.appDomainFactory = appDomainFactory;
            this.proxyableFactory = proxyableFactory;
        }

        public delegate TResult RemoteOperation(TProxyable proxyable);
        public delegate TResult FailureCallback(Exception ex, string binaryPath);

        public TResult Run(string binaryPath, RemoteOperation operation, FailureCallback fail)
        {
            try
            {
                using (var targetDomain = appDomainFactory.Create(binaryPath))
                using (var proxyable = proxyableFactory.CreateProxy(targetDomain))
                {
                    TResult result = operation(proxyable);

                    return result;
                }
            }
            catch (Exception ex)
            {
                TResult result = fail(ex, binaryPath);

                return result;
            }
        }

        readonly IAppDomainFactory appDomainFactory;
        readonly IProxyableFactory<TProxyable> proxyableFactory;

        public TResult Run(string binaryPath, Func<TProxyable, TResult> operation, Func<Exception, string, TResult> fail)
        {
            throw new NotImplementedException();
        }
    }
}
