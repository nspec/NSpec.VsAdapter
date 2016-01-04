using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.TestAdapter
{
    [FileExtension(Constants.DllExtension)]
    [FileExtension(Constants.ExeExtension)]
    [DefaultExecutorUri(Constants.ExecutorUriString)]
    public class NSpecTestDiscoverer : ITestDiscoverer
    {
        // used by Visual Studio test infrastructure
        public NSpecTestDiscoverer() : this(new CrossDomainTestDiscoverer()) { }

        // used to test this adapter
        public NSpecTestDiscoverer(ICrossDomainTestDiscoverer crossDomainTestDiscoverer)
        {
            this.crossDomainTestDiscoverer = crossDomainTestDiscoverer;
        }

        public void DiscoverTests(
            IEnumerable<string> sources, 
            IDiscoveryContext discoveryContext, 
            IMessageLogger logger, 
            ITestCaseDiscoverySink discoverySink)
        {
            var specificationGroups =
                from assemblyPath in sources
                select crossDomainTestDiscoverer.Discover(assemblyPath);

            var specifications = specificationGroups.SelectMany(group => group);

            var testCases =
                from spec in specifications
                select new TestCase(spec.FullName, Constants.ExecutorUri, spec.SourceFilePath);

            testCases.Do(discoverySink.SendTestCase);
        }

        readonly ICrossDomainTestDiscoverer crossDomainTestDiscoverer;
    }
}
