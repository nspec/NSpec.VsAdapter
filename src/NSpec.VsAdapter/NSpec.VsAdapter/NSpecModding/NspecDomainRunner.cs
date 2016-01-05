using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.NSpecModding
{
    [Serializable]
    public class NspecDomainRunner<TInvocation, TResult> : INspecDomainRunner<TInvocation, TResult>
    {
        // initial implementation taken from http://thevalerios.net/matt/2008/06/run-anonymous-methods-in-another-appdomain/

        public NspecDomainRunner(IAppDomainFactory appDomainFactory)
        {
            this.appDomainFactory = appDomainFactory;
        }

        public TResult Run(string assemblyPath, TInvocation invocation, Func<TInvocation, TResult> outputSelector)
        {
            AppDomain targetDomain = null;

            TResult result;

            try
            {
                targetDomain = appDomainFactory.Create(assemblyPath);

                var marshalingType = typeof(MarshalingWrapper<TInvocation, TResult>);

                var marshalingTypeName = marshalingType.FullName;

                var marshalingAssemblyName = marshalingType.Assembly.GetName().Name;

                var marshalingWrapper = (MarshalingWrapper<TInvocation, TResult>)targetDomain
                    .CreateInstanceAndUnwrap(marshalingAssemblyName, marshalingTypeName);

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
    }
}
