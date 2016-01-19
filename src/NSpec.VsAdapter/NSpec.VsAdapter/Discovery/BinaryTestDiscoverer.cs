using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Discovery
{
    public class BinaryTestDiscoverer : IBinaryTestDiscoverer
    {
        public BinaryTestDiscoverer(ICrossDomainCollector crossDomainCollector)
        {
            this.crossDomainCollector = crossDomainCollector;
        }

        public IEnumerable<DiscoveredExample> Discover(string binaryPath, 
            IOutputLogger logger, IReplayLogger crossDomainLogger)
        {
            IEnumerable<DiscoveredExample> discoveredExamples;

            try
            {
                logger.Debug(String.Format("Discovering tests in binary: '{0}'", binaryPath));

                var logRecorder = new LogRecorder();

                // TODO exclude assembly/binary right away if nspec.dll is not in the same path (or sub-path?)

                var collectorInvocation = new CollectorInvocation(binaryPath, logRecorder);

                discoveredExamples = crossDomainCollector.Run(binaryPath, collectorInvocation.Collect);

                var logReplayer = new LogReplayer(crossDomainLogger);

                logReplayer.Replay(logRecorder);

                logger.Debug(String.Format("Found {0} tests", discoveredExamples.Count()));

                return discoveredExamples;
            }
            catch (Exception ex)
            {
                // report problem and return for the next assembly, without crashing the test discovery process

                var message = String.Format("Exception thrown while discovering tests in binary '{0}'", binaryPath);
                
                logger.Error(ex, message);

                discoveredExamples = new DiscoveredExample[0];
            }

            return discoveredExamples;
        }

        readonly ICrossDomainCollector crossDomainCollector;
    }
}
