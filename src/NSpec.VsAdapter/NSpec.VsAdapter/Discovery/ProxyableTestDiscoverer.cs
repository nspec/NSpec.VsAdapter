using NSpec.VsAdapter.CrossDomain;
using NSpec.VsAdapter.Logging;
using System;
using System.Linq;

namespace NSpec.VsAdapter.Discovery
{
    public class ProxyableTestDiscoverer : Proxyable, IProxyableTestDiscoverer
    {
        public DiscoveredExample[] Discover(string binaryPath, ICrossDomainLogger logger)
        {
            logger.Debug(String.Format("Start discovering tests locally in binary '{0}'", binaryPath));

            DiscoveredExample[] discoveredExampleArray;

            try
            {
                var exampleFinder = new ExampleFinder();

                var examples = exampleFinder.Find(binaryPath);

                var debugInfoProvider = new DebugInfoProvider(binaryPath, logger);

                var discoveredExampleMapper = new DiscoveredExampleMapper(binaryPath, debugInfoProvider);

                var discoveredExamples = examples.Select(discoveredExampleMapper.FromExample);

                discoveredExampleArray = discoveredExamples.ToArray();
            }
            catch (Exception ex)
            {
                // report problem and return, without letting exception cross app domain boundary

                discoveredExampleArray = new DiscoveredExample[0];

                var exInfo = new ExceptionLogInfo(ex);
                var message = String.Format("Exception thrown while discovering tests locally in binary '{0}'", binaryPath);

                logger.Error(exInfo, message);
            }

            logger.Debug(String.Format("Finish discovering {0} tests locally in binary '{1}'", discoveredExampleArray.Length, binaryPath));

            return discoveredExampleArray;
        }
    }
}
