using Autofac;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using NSpec.VsAdapter.Discovery;
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
    public class NSpecTestDiscoverer : ITestDiscoverer, IDisposable
    {
        // used by Visual Studio test infrastructure
        public NSpecTestDiscoverer() : this(
            DIContainer.Instance.Discoverer.Resolve<ICrossDomainTestDiscoverer>(),
            DIContainer.Instance.Discoverer.Resolve<ITestCaseMapper>(),
            DIContainer.Instance.Discoverer.Resolve<ILoggerFactory>())
        {
        }

        // used to test this adapter
        public NSpecTestDiscoverer(
            ICrossDomainTestDiscoverer crossDomainTestDiscoverer, 
            ITestCaseMapper testCaseMapper,
            ILoggerFactory loggerFactory)
        {
            this.crossDomainTestDiscoverer = crossDomainTestDiscoverer;
            this.testCaseMapper = testCaseMapper;
            this.loggerFactory = loggerFactory;
        }

        public void Dispose()
        {
            DIContainer.Instance.Discoverer.Dispose();
        }

        public void DiscoverTests(
            IEnumerable<string> sources, 
            IDiscoveryContext discoveryContext, 
            IMessageLogger logger, 
            ITestCaseDiscoverySink discoverySink)
        {
            // TODO implement custom runtime TestSettings, e.g. to enable debug logging
            // E.g. as https://github.com/mmanela/chutzpah/blob/master/VS2012.TestAdapter/ChutzpahTestDiscoverer.cs

            var outputLogger = loggerFactory.CreateOutput(logger);

            outputLogger.Info("Discovery started");

            var specificationGroups =
                from assemblyPath in sources
                select crossDomainTestDiscoverer.Discover(assemblyPath, outputLogger, outputLogger);

            var specifications = specificationGroups.SelectMany(group => group);

            var testCases = specifications.Select(testCaseMapper.FromSpecification);

            testCases.Do(discoverySink.SendTestCase);

            outputLogger.Info("Discovery finished");
        }

        readonly ICrossDomainTestDiscoverer crossDomainTestDiscoverer;
        readonly ITestCaseMapper testCaseMapper;
        readonly ILoggerFactory loggerFactory;
    }
}
