using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSpec.VsAdapter.TestAdapter
{
    public interface IMultiSourceTestDiscoverer
    {
        void DiscoverTests(ITestCaseDiscoverySink discoverySink, IMessageLogger messageLogger);
    }
}
