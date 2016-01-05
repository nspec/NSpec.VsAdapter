using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.NSpecModding
{
    [Serializable]
    public class CrossDomainRunner<TInvocation, TResult> : ICrossDomainRunner<TInvocation, TResult>
    {
        // initial implementation taken from 
        // http://thevalerios.net/matt/2008/06/run-anonymous-methods-in-another-appdomain/

        public CrossDomainRunner(
            IAppDomainFactory appDomainFactory, 
            IMarshalingFactory<TInvocation, TResult> marshalingFactory)
        {
            this.appDomainFactory = appDomainFactory;
            this.marshalingFactory = marshalingFactory;
        }

        public TResult Run(string assemblyPath, TInvocation invocation, Func<TInvocation, TResult> outputSelector)
        {
            AppDomain targetDomain = null;

            TResult result;

            try
            {
                targetDomain = appDomainFactory.Create(assemblyPath);

                var marshalingWrapper = marshalingFactory.CreateWrapper(targetDomain);

                result = marshalingWrapper.Execute(invocation, outputSelector);
            }
            catch (Exception)
            {
                // just swallow exception for time being, until a cross-domain logging facility is implemented

                result = default(TResult);
            }

            if (targetDomain != null)
            {
                AppDomain.Unload(targetDomain);
            }

            return result;
        }

        readonly IAppDomainFactory appDomainFactory;
        readonly IMarshalingFactory<TInvocation, TResult> marshalingFactory;
    }
}
