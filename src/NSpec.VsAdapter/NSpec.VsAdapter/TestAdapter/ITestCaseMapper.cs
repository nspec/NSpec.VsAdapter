using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NSpec.VsAdapter.Discovery;

namespace NSpec.VsAdapter.TestAdapter
{
    public interface ITestCaseMapper
    {
        TestCase FromDiscoveredExample(DiscoveredExample spec);
    }
}
