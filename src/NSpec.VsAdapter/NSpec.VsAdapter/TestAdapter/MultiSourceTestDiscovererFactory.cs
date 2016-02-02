using NSpec.VsAdapter.Discovery;
using NSpec.VsAdapter.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.TestAdapter
{
    public class MultiSourceTestDiscovererFactory : IMultiSourceTestDiscovererFactory
    {
        public MultiSourceTestDiscovererFactory(
            IBinaryTestDiscoverer binaryTestDiscoverer,
            ITestCaseMapper testCaseMapper,
            ILoggerFactory loggerFactory)
        {
            this.binaryTestDiscoverer = binaryTestDiscoverer;
            this.testCaseMapper = testCaseMapper;
            this.loggerFactory = loggerFactory;
        }

        public IMultiSourceTestDiscoverer Create(IEnumerable<string> sources)
        {
            return new MultiSourceTestDiscoverer(sources, binaryTestDiscoverer, testCaseMapper, loggerFactory);
        }

        readonly IBinaryTestDiscoverer binaryTestDiscoverer;
        readonly ITestCaseMapper testCaseMapper;
        readonly ILoggerFactory loggerFactory;
    }
}
