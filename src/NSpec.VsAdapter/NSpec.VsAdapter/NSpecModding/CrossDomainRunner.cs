using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.NSpecModding
{
    [Serializable]
    public class CrossDomainRunner<TResult> : ICrossDomainRunner<TResult>
    {
        // initial implementation taken from 
        // http://thevalerios.net/matt/2008/06/run-anonymous-methods-in-another-appdomain/

        public CrossDomainRunner(
            IAppDomainFactory appDomainFactory, 
            IMarshalingFactory<TResult> marshalingFactory)
        {
            this.appDomainFactory = appDomainFactory;
            this.marshalingFactory = marshalingFactory;
        }

        public virtual TResult Run(string assemblyPath, Func<TResult> targetOperation)
        {
            ITargetAppDomain targetDomain = null;

            TResult result;

            try
            {
                targetDomain = appDomainFactory.Create(assemblyPath);

                var crossDomainProxy = marshalingFactory.CreateProxy(targetDomain);

                result = crossDomainProxy.Execute(targetOperation);
            }
            catch (Exception)
            {
                // just swallow exception for time being, until a cross-domain logging facility is implemented

                result = default(TResult);
            }

            if (targetDomain != null)
            {
                targetDomain.Unload();
            }

            return result;
        }

        readonly IAppDomainFactory appDomainFactory;
        readonly IMarshalingFactory<TResult> marshalingFactory;
    }
}
