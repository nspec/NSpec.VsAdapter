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
        public CrossDomainTestDiscoverer(ICrossDomainCollector crossDomainCollector)
        {
            this.crossDomainCollector = crossDomainCollector;
        }

        public IEnumerable<NSpecSpecification> Discover(string assemblyPath, IOutputLogger logger)
        {
            IEnumerable<NSpecSpecification> specifications;

            try
            {
                logger.Debug(String.Format("Processing container: '{0}'", assemblyPath));

                var collectorInvocation = new CollectorInvocation(assemblyPath);

                specifications = crossDomainCollector.Run(assemblyPath, logger, collectorInvocation.Collect);

                logger.Debug(String.Format("Found {0} specs", specifications.Count()));

                return specifications;
            }
            catch (Exception ex)
            {
                // report problem and return for the next assembly, without crashing the container discovery process

                var message = String.Format("Exception thrown while discovering tests in container '{0}'", assemblyPath);
                
                logger.Error(ex, message);

                specifications = new NSpecSpecification[0];
            }

            return specifications;
        }

        readonly ICrossDomainCollector crossDomainCollector;
    }
}
