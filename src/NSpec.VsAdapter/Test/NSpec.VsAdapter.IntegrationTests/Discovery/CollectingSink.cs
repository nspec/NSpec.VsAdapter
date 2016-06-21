using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using System.Collections.Generic;

namespace NSpec.VsAdapter.IntegrationTests.Discovery
{
    internal class CollectingSink : ITestCaseDiscoverySink
    {
        public CollectingSink()
        {
            TestCases = new List<TestCase>();
        }

        public List<TestCase> TestCases { get; private set; }

        public void SendTestCase(TestCase discoveredTestCase)
        {
            TestCases.Add(discoveredTestCase);
        }
    }
}
