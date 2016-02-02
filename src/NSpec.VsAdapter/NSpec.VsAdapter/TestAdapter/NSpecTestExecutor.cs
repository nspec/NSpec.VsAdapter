using Autofac;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using NSpec.VsAdapter.Execution;
using NSpec.VsAdapter.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.TestAdapter
{
    [ExtensionUri(Constants.ExecutorUriString)]
    public class NSpecTestExecutor : ITestExecutor, IDisposable
    {
        // used by Visual Studio test infrastructure, by integration tests
        public NSpecTestExecutor()
        {
            var scope = DIContainer.Instance.BeginScope();

            disposable = scope;

            this.binaryTestExecutor = scope.Resolve<IBinaryTestExecutor>();
            this.progressRecorderFactory = scope.Resolve<IProgressRecorderFactory>();
            this.loggerFactory = scope.Resolve<ILoggerFactory>();
        }

        // used by unit tests
        public NSpecTestExecutor(
            IBinaryTestExecutor binaryTestExecutor,
            IProgressRecorderFactory progressRecorderFactory,
            ILoggerFactory loggerFactory)
        {
            this.binaryTestExecutor = binaryTestExecutor;
            this.progressRecorderFactory = progressRecorderFactory;
            this.loggerFactory = loggerFactory;

            disposable = Disposable.Empty;
        }

        public void Dispose()
        {
            disposable.Dispose();
        }

        public void RunTests(IEnumerable<string> sources, IRunContext runContext, IFrameworkHandle frameworkHandle)
        {
            var testableItems = sources.Select(source => new SourceTestableItem(source));

            ProcessTestableItems(testableItems, frameworkHandle, "source paths");
        }

        public void RunTests(IEnumerable<TestCase> tests, IRunContext runContext, IFrameworkHandle frameworkHandle)
        {
            var testCaseGroupsBySource = tests.GroupBy(t => t.Source);

            var testableItems = testCaseGroupsBySource.Select(group => new TestCaseGroupTestableItem(group));

            ProcessTestableItems(testableItems, frameworkHandle, "TestCases");
        }

        public void Cancel()
        {
            isCanceled = true;
        }

        void ProcessTestableItems(IEnumerable<ITestableItem> testableItems, IFrameworkHandle frameworkHandle, string methodDescription)
        {
            var outputLogger = loggerFactory.CreateOutput(frameworkHandle);

            outputLogger.Info(String.Format("Execution by {0} started", methodDescription));

            isCanceled = false;

            using (var progressRecorder = progressRecorderFactory.Create((ITestExecutionRecorder)frameworkHandle))
            using (var crossDomainLogger = new CrossDomainLogger(outputLogger))
            {
                foreach (var item in testableItems)
                {
                    if (isCanceled)
                    {
                        break;
                    }

                    progressRecorder.BinaryPath = item.BinaryPath;

                    item.Execute(binaryTestExecutor, progressRecorder, outputLogger, crossDomainLogger);
                }
            }

            outputLogger.Info(String.Format("Execution by {0} finished", methodDescription));
        }

        interface ITestableItem
        {
            string BinaryPath { get; }

            void Execute(
                IBinaryTestExecutor binaryTestExecutor,
                IProgressRecorder progressRecorder,
                IOutputLogger outputLogger, ICrossDomainLogger crossDomainLogger);
        }

        class SourceTestableItem : ITestableItem
        {
            public SourceTestableItem(string source)
            {
                this.source = source;
            }

            public string BinaryPath { get { return source; } }

            public void Execute(
                IBinaryTestExecutor binaryTestExecutor, 
                IProgressRecorder progressRecorder, 
                IOutputLogger outputLogger, ICrossDomainLogger crossDomainLogger)
            {
                binaryTestExecutor.Execute(BinaryPath, progressRecorder, outputLogger, crossDomainLogger);
            }

            readonly string source;
        }

        class TestCaseGroupTestableItem : ITestableItem
        {
            public TestCaseGroupTestableItem(IGrouping<string, TestCase> testCaseGroup)
            {
                this.testCaseGroup = testCaseGroup;
            }

            public string BinaryPath { get { return testCaseGroup.Key; } }

            public void Execute(
                IBinaryTestExecutor binaryTestExecutor, 
                IProgressRecorder progressRecorder, 
                IOutputLogger outputLogger, ICrossDomainLogger crossDomainLogger)
            {
                var testCaseFullNames = testCaseGroup.Select(tc => tc.FullyQualifiedName);

                binaryTestExecutor.Execute(BinaryPath, testCaseFullNames, progressRecorder, outputLogger, crossDomainLogger);
            }

            readonly IGrouping<string, TestCase> testCaseGroup;
        }

        bool isCanceled;

        readonly IBinaryTestExecutor binaryTestExecutor;
        readonly IProgressRecorderFactory progressRecorderFactory;
        readonly ILoggerFactory loggerFactory;
        readonly IDisposable disposable;
    }
}
