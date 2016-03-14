using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.CrossDomain
{
    public class CrossDomainRunner<TResult> : ICrossDomainRunner<TResult>
    {
        // initial implementation taken from 
        // http://thevalerios.net/matt/2008/06/run-anonymous-methods-in-another-appdomain/

        public CrossDomainRunner(IAppDomainFactory appDomainFactory, IProxyFactory<TResult> proxyFactory)
        {
            this.appDomainFactory = appDomainFactory;
            this.proxyFactory = proxyFactory;
        }

        public virtual TResult Run(string binaryPath, Func<TResult> targetOperation)
        {
            using (var targetDomain = appDomainFactory.Create(binaryPath))
            using (var crossDomainProxy = proxyFactory.CreateProxy(targetDomain))
            {
                TResult result = crossDomainProxy.Execute(targetOperation);

                return result;
            }
        }

        readonly IAppDomainFactory appDomainFactory;
        readonly IProxyFactory<TResult> proxyFactory;
    }
}
