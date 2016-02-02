using Autofac;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using NSpec.VsAdapter.Discovery;
using NSpec.VsAdapter.Logging;
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

            binaryTestDiscoverer = scope.Resolve<IBinaryTestDiscoverer>();
            testCaseMapper = scope.Resolve<ITestCaseMapper>();
            loggerFactory = scope.Resolve<ILoggerFactory>();
        }

        // used by unit tests
        public NSpecTestDiscoverer(
            IBinaryTestDiscoverer binaryTestDiscoverer, 
            ITestCaseMapper testCaseMapper,
            ILoggerFactory loggerFactory)
        {
            this.binaryTestDiscoverer = binaryTestDiscoverer;
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
            // TODO extract into a MultiSourceTestDiscoverer

            var outputLogger = loggerFactory.CreateOutput(logger);

            outputLogger.Info("Discovery started");

            IEnumerable<IEnumerable<DiscoveredExample>> groupedSpecifications;

            using (var crossDomainLogger = new CrossDomainLogger(outputLogger))
            {
                groupedSpecifications =
                    from binaryPath in sources
                    select binaryTestDiscoverer.Discover(binaryPath, outputLogger, crossDomainLogger);
            }

            var specifications = groupedSpecifications.SelectMany(group => group);

            var testCases = specifications.Select(testCaseMapper.FromDiscoveredExample);

            testCases.Do(discoverySink.SendTestCase);

            outputLogger.Info("Discovery finished");
        }

        readonly IBinaryTestDiscoverer binaryTestDiscoverer;
        readonly ITestCaseMapper testCaseMapper;
        readonly ILoggerFactory loggerFactory;
        readonly IDisposable disposable;
    }
}
