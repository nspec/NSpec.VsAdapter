using Autofac;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using NSpec.VsAdapter.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
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

            this.crossDomainTestExecutor = scope.Resolve<ICrossDomainTestExecutor>();
            this.executionObserverFactory = scope.Resolve<IExecutionObserverFactory>();
            this.loggerFactory = scope.Resolve<ILoggerFactory>();
        }

        // used by unit tests
        public NSpecTestExecutor(
            ICrossDomainTestExecutor crossDomainTestExecutor,
            IExecutionObserverFactory executionObserverFactory,
            ILoggerFactory loggerFactory)
        {
            this.crossDomainTestExecutor = crossDomainTestExecutor;
            this.executionObserverFactory = executionObserverFactory;
            this.loggerFactory = loggerFactory;
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

            sources.Do(assemblyPath =>
                {
                    crossDomainTestExecutor.Execute(assemblyPath, executionObserver, outputLogger, outputLogger);
                });

            outputLogger.Info("Execution by source paths finished");
        }

        public void RunTests(IEnumerable<TestCase> tests, IRunContext runContext, IFrameworkHandle frameworkHandle)
        {
            // TODO implement custom runtime TestSettings, e.g. to enable debug logging
            // E.g. as https://github.com/mmanela/chutzpah/blob/master/VS2012.TestAdapter/ChutzpahTestDiscoverer.cs

            var outputLogger = loggerFactory.CreateOutput(frameworkHandle);

            outputLogger.Info("Execution by TestCases started");

            outputLogger.Error("Execution by TestCases NOT IMPLEMENTED yet"); // TODO

            outputLogger.Info("Execution by TestCases finished");
        }

        public void Cancel()
        {
            throw new NotImplementedException();
        }

        readonly ICrossDomainTestExecutor crossDomainTestExecutor;
        readonly IExecutionObserverFactory executionObserverFactory;
        readonly ILoggerFactory loggerFactory;
        readonly IDisposable disposable;
    }
}
