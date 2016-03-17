using NSpec.VsAdapter.CrossDomain;
using NSpec.VsAdapter.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Discovery.Target
{
    public class ProxyableTestDiscoverer : Proxyable, IProxyableTestDiscoverer
    {
        // Cross-domain instantiation requires a default constructor
        public ProxyableTestDiscoverer()
        {
            this.exampleFinder = new ExampleFinder();
            this.debugInfoProviderFactory = new DebugInfoProviderFactory();
            this.discoveredExampleMapperFactory = new DiscoveredExampleMapperFactory();
        }

        // Unit tests need a constructor with injected dependencies
        public ProxyableTestDiscoverer(
            IExampleFinder exampleFinder, 
            IDebugInfoProviderFactory debugInfoProviderFactory,
            IDiscoveredExampleMapperFactory discoveredExampleMapperFactory)
        {
            this.exampleFinder = exampleFinder;
            this.debugInfoProviderFactory = debugInfoProviderFactory;
            this.discoveredExampleMapperFactory = discoveredExampleMapperFactory;
        }

        public DiscoveredExample[] Discover(string binaryPath, ICrossDomainLogger logger)
        {
            logger.Debug(String.Format("Start discovering tests locally in binary '{0}'", binaryPath));

            DiscoveredExample[] discoveredExampleArray;

            try
            {
                var examples = exampleFinder.Find(binaryPath);

                var debugInfoProvider = debugInfoProviderFactory.Create(binaryPath, logger);

                var discoveredExampleMapper = discoveredExampleMapperFactory.Create(binaryPath, debugInfoProvider);

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

        readonly IExampleFinder exampleFinder;
        readonly IDebugInfoProviderFactory debugInfoProviderFactory;
        readonly IDiscoveredExampleMapperFactory discoveredExampleMapperFactory;
    }
}
