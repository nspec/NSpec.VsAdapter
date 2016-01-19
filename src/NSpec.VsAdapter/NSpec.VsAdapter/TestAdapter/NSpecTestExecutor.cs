using Autofac;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using NSpec.VsAdapter.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.TestAdapter
{
    [DefaultExecutorUri(Constants.ExecutorUriString)]
    public class NSpecTestExecutor : ITestExecutor, IDisposable
    {
        // used by Visual Studio test infrastructure, by integration tests
        public NSpecTestExecutor()
        {
            var scope = DIContainer.Instance.BeginScope();

            disposable = scope;

            this.binaryTestExecutor = scope.Resolve<IBinaryTestExecutor>();
            this.executionObserverFactory = scope.Resolve<IExecutionObserverFactory>();
            this.loggerFactory = scope.Resolve<ILoggerFactory>();
        }

        // used by unit tests
        public NSpecTestExecutor(
            IBinaryTestExecutor binaryTestExecutor,
            IExecutionObserverFactory executionObserverFactory,
            ILoggerFactory loggerFactory)
        {
            this.binaryTestExecutor = binaryTestExecutor;
            this.executionObserverFactory = executionObserverFactory;
            this.loggerFactory = loggerFactory;

            disposable = Disposable.Empty;
        }

        public void Dispose()
        {
            disposable.Dispose();
        }

        public void RunTests(IEnumerable<string> sources, IRunContext runContext, IFrameworkHandle frameworkHandle)
        {
            // TODO implement custom runtime TestSettings, e.g. to enable debug logging
            // E.g. as https://github.com/mmanela/chutzpah/blob/master/VS2012.TestAdapter/ChutzpahTestDiscoverer.cs

            var outputLogger = loggerFactory.CreateOutput((IMessageLogger)frameworkHandle);

            outputLogger.Info("Execution by source paths started");

            var executionObserver = executionObserverFactory.Create((ITestExecutionRecorder)frameworkHandle);

            foreach (var binaryPath in sources)
            {
                executionObserver.BinaryPath = binaryPath;

                binaryTestExecutor.Execute(binaryPath, executionObserver, outputLogger, outputLogger);
            }

            outputLogger.Info("Execution by source paths finished");
        }

        public void RunTests(IEnumerable<TestCase> tests, IRunContext runContext, IFrameworkHandle frameworkHandle)
        {
            // TODO implement custom runtime TestSettings, e.g. to enable debug logging
            // E.g. as https://github.com/mmanela/chutzpah/blob/master/VS2012.TestAdapter/ChutzpahTestDiscoverer.cs

            var outputLogger = loggerFactory.CreateOutput(frameworkHandle);

            outputLogger.Info("Execution by TestCases started");

            var executionObserver = executionObserverFactory.Create((ITestExecutionRecorder)frameworkHandle);

            var testCaseGroupsBySource = tests.GroupBy(t => t.Source);

            foreach (var group in testCaseGroupsBySource)
            {
                string binaryPath = group.Key;

                var testCaseFullNames = group.Select(tc => tc.FullyQualifiedName);

                executionObserver.BinaryPath = binaryPath;

                binaryTestExecutor.Execute(binaryPath, testCaseFullNames, executionObserver, outputLogger, outputLogger);
            }

            outputLogger.Info("Execution by TestCases finished");
        }

        public void Cancel()
        {
            throw new NotImplementedException();
        }

        readonly IBinaryTestExecutor binaryTestExecutor;
        readonly IExecutionObserverFactory executionObserverFactory;
        readonly ILoggerFactory loggerFactory;
        readonly IDisposable disposable;
    }
}
