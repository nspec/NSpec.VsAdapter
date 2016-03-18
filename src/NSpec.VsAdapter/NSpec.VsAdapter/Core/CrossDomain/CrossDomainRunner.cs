using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Core.CrossDomain
{
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

        public TResult Run(string binaryPath, 
            Func<TProxyable, TResult> operation, 
            Func<Exception, string, TResult> fail)
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
    }
}
