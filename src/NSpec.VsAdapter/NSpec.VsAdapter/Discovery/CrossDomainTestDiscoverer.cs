using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Discovery
{
    public class CrossDomainTestDiscoverer : ICrossDomainTestDiscoverer
    {
        public CrossDomainTestDiscoverer(ICrossDomainCollector crossDomainCollector)
        {
            this.crossDomainCollector = crossDomainCollector;
        }

        public IEnumerable<NSpecSpecification> Discover(string assemblyPath, 
            IOutputLogger logger, IReplayLogger crossDomainLogger)
        {
            IEnumerable<NSpecSpecification> specifications;

            try
            {
                logger.Debug(String.Format("Discovering tests in binary: '{0}'", assemblyPath));

                var logRecorder = new LogRecorder();

                // TODO exclude assembly/binary right away if nspec.dll is not in the same path (or sub-path?)

                var collectorInvocation = new CollectorInvocation(assemblyPath, logRecorder);

                specifications = crossDomainCollector.Run(assemblyPath, collectorInvocation.Collect);

                var logReplayer = new LogReplayer(crossDomainLogger);

                logReplayer.Replay(logRecorder);

                logger.Debug(String.Format("Found {0} tests", specifications.Count()));

                return specifications;
            }
            catch (Exception ex)
            {
                // report problem and return for the next assembly, without crashing the test discovery process

                var message = String.Format("Exception thrown while discovering tests in binary '{0}'", assemblyPath);
                
                logger.Error(ex, message);

                specifications = new NSpecSpecification[0];
            }

            return specifications;
        }

        readonly ICrossDomainCollector crossDomainCollector;
    }
}
