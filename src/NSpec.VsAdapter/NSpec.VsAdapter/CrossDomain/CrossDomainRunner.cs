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

        public CrossDomainRunner(IAppDomainFactory appDomainFactory, IMarshalingFactory<TResult> marshalingFactory)
        {
            this.appDomainFactory = appDomainFactory;
            this.marshalingFactory = marshalingFactory;
        }

        public virtual TResult Run(string assemblyPath, Func<TResult> targetOperation)
        {
            using (var targetDomain = appDomainFactory.Create(assemblyPath))
            using (var crossDomainProxy = marshalingFactory.CreateProxy(targetDomain))
            {
                TResult result = crossDomainProxy.Execute(targetOperation);

                return result;
            }
        }

        readonly IAppDomainFactory appDomainFactory;
        readonly IMarshalingFactory<TResult> marshalingFactory;
    }
}
