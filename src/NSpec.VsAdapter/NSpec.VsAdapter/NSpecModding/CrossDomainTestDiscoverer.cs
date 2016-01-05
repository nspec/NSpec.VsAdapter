using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using NSpec.VsAdapter.TestAdapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.NSpecModding
{
    public class CrossDomainTestDiscoverer : ICrossDomainTestDiscoverer
    {
        public CrossDomainTestDiscoverer(ICrossDomainRunner crossDomainRunner)
        {
            this.crossDomainRunner = crossDomainRunner;
        }

        public IEnumerable<NSpecSpecification> Discover(string assemblyPath, IMessageLogger logger)
        {
            IEnumerable<NSpecSpecification> specifications;

            try
            {
                // TODO this should be at debug level, through a custom logger
                logger.SendMessage(TestMessageLevel.Informational, String.Format("Processing container: '{0}'", assemblyPath));

                var collectorInvocation = new CollectorInvocation(assemblyPath);

                specifications = 
                    crossDomainRunner.Run(assemblyPath, collectorInvocation, invocation => invocation.Collect());

                // TODO this should be at debug level, through a custom logger
                logger.SendMessage(TestMessageLevel.Informational, String.Format("Found {0} specs", specifications.Count()));

                return specifications;
            }
            catch (Exception ex)
            {
                // Report problem and return for the next assembly, without crashing the discovery process

                var message = String.Format(
                    "Exception found while discovering tests in source '{0}': {1}", assemblyPath, ex);
                
                logger.SendMessage(TestMessageLevel.Error, message);

                specifications = new NSpecSpecification[0];
            }

            return specifications;
        }

        readonly ICrossDomainRunner crossDomainRunner;
    }
}
