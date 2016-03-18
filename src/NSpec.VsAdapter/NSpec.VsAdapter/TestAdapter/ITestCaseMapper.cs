using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NSpec.VsAdapter.Core.Discovery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSpec.VsAdapter.TestAdapter
{
    public interface ITestCaseMapper
    {
        TestCase FromDiscoveredExample(DiscoveredExample spec);
    }
}
