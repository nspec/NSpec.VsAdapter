using NSpec.VsAdapter.Core.Discovery;
using NSpec.VsAdapter.Logging;
using NSpec.VsAdapter.Settings;
using System.Collections.Generic;

namespace NSpec.VsAdapter.TestAdapter
{
    public class MultiSourceTestDiscovererFactory : IMultiSourceTestDiscovererFactory
    {
        public MultiSourceTestDiscovererFactory(
            IBinaryTestDiscoverer binaryTestDiscoverer,
            ITestCaseMapper testCaseMapper,
            ISettingsRepository settingsRepository,
            ILoggerFactory loggerFactory)
        {
            this.binaryTestDiscoverer = binaryTestDiscoverer;
            this.testCaseMapper = testCaseMapper;
            this.settingsRepository = settingsRepository;
            this.loggerFactory = loggerFactory;
        }

        public IMultiSourceTestDiscoverer Create(IEnumerable<string> sources)
        {
            return new MultiSourceTestDiscoverer(sources, binaryTestDiscoverer, testCaseMapper, settingsRepository, loggerFactory);
        }

        readonly IBinaryTestDiscoverer binaryTestDiscoverer;
        readonly ITestCaseMapper testCaseMapper;
        readonly ISettingsRepository settingsRepository;
        readonly ILoggerFactory loggerFactory;
    }
}
