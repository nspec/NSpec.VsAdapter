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

        public virtual TResult Run(string assemblyPath, IOutputLogger logger, Func<TResult> targetOperation)
        {
            ITargetAppDomain targetDomain = null;

            TResult result;

            try
            {
                targetDomain = appDomainFactory.Create(assemblyPath);

                var crossDomainProxy = marshalingFactory.CreateProxy(targetDomain);

                result = crossDomainProxy.Execute(targetOperation);
            }
            catch (Exception ex)
            {
                // report problem and rethrow, cleaning up resources before leaving

                var message = String.Format("Exception found while executing across domain in binary '{0}'", assemblyPath);

                logger.Error(ex, message);

                throw;
            }
            finally
            {
                if (targetDomain != null)
                {
                    targetDomain.Unload();
                }
            }

            return result;
        }

        readonly IAppDomainFactory appDomainFactory;
        readonly IMarshalingFactory<TResult> marshalingFactory;
    }
}
