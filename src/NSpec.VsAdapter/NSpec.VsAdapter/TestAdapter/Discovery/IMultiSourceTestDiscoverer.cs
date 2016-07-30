using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace NSpec.VsAdapter.TestAdapter.Discovery
{
    public interface IMultiSourceTestDiscoverer
    {
        void DiscoverTests(ITestCaseDiscoverySink discoverySink, IMessageLogger messageLogger, IDiscoveryContext discoveryContext);
    }
}
