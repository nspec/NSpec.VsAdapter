using Autofac;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using NSpec.VsAdapter.Discovery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.TestAdapter
{
    [FileExtension(Constants.DllExtension)]
    [FileExtension(Constants.ExeExtension)]
    [DefaultExecutorUri(Constants.ExecutorUriString)]
    public class NSpecTestDiscoverer : ITestDiscoverer, IDisposable
    {
        // used by Visual Studio test infrastructure, by integration tests
        public NSpecTestDiscoverer()
        {
            var scope = DIContainer.Instance.BeginScope();

            disposable = scope;

            crossDomainTestDiscoverer = scope.Resolve<ICrossDomainTestDiscoverer>();
            testCaseMapper = scope.Resolve<ITestCaseMapper>();
            loggerFactory = scope.Resolve<ILoggerFactory>();
        }

        // used by unit tests
        public NSpecTestDiscoverer(
            ICrossDomainTestDiscoverer crossDomainTestDiscoverer, 
            ITestCaseMapper testCaseMapper,
            ILoggerFactory loggerFactory)
        {
            this.crossDomainTestDiscoverer = crossDomainTestDiscoverer;
            this.testCaseMapper = testCaseMapper;
            this.loggerFactory = loggerFactory;

            disposable = Disposable.Empty;
        }

        public void Dispose()
        {
            disposable.Dispose();
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

            var groupedSpecifications =
                from assemblyPath in sources
                select crossDomainTestDiscoverer.Discover(assemblyPath, outputLogger, outputLogger);

            var specifications = groupedSpecifications.SelectMany(group => group);

            var testCases = specifications.Select(testCaseMapper.FromSpecification);

            testCases.Do(discoverySink.SendTestCase);

            outputLogger.Info("Discovery finished");
        }

        readonly ICrossDomainTestDiscoverer crossDomainTestDiscoverer;
        readonly ITestCaseMapper testCaseMapper;
        readonly ILoggerFactory loggerFactory;
        readonly IDisposable disposable;
    }
}
