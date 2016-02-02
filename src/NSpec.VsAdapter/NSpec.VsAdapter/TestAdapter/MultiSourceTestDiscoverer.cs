using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using NSpec.VsAdapter.Discovery;
using NSpec.VsAdapter.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.TestAdapter
{
    public class MultiSourceTestDiscoverer : IMultiSourceTestDiscoverer
    {
        public MultiSourceTestDiscoverer(IEnumerable<string> sources,
            IBinaryTestDiscoverer binaryTestDiscoverer,
            ITestCaseMapper testCaseMapper,
            ILoggerFactory loggerFactory)
        {
            this.sources = sources;
            this.binaryTestDiscoverer = binaryTestDiscoverer;
            this.testCaseMapper = testCaseMapper;
            this.loggerFactory = loggerFactory;
        }

        public void DiscoverTests(ITestCaseDiscoverySink discoverySink, IMessageLogger messageLogger)
        {
            var outputLogger = loggerFactory.CreateOutput(messageLogger);

            outputLogger.Info("Discovery started");

            IEnumerable<IEnumerable<DiscoveredExample>> groupedSpecifications;

            using (var crossDomainLogger = new CrossDomainLogger(outputLogger))
            {
                groupedSpecifications =
                    from binaryPath in sources
                    select binaryTestDiscoverer.Discover(binaryPath, outputLogger, crossDomainLogger);
            }

            var specifications = groupedSpecifications.SelectMany(group => group);

            var testCases = specifications.Select(testCaseMapper.FromDiscoveredExample);

            testCases.Do(discoverySink.SendTestCase);

            outputLogger.Info("Discovery finished");
        }

        readonly IEnumerable<string> sources;
        readonly IBinaryTestDiscoverer binaryTestDiscoverer;
        readonly ITestCaseMapper testCaseMapper;
        readonly ILoggerFactory loggerFactory;
    }
}
