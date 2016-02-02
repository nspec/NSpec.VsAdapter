using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using NSpec.VsAdapter.Execution;
using NSpec.VsAdapter.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.TestAdapter
{
    public class MultiSourceTestExecutor : IMultiSourceTestExecutor
    {
        public MultiSourceTestExecutor(IEnumerable<string> sources,
            IBinaryTestExecutor binaryTestExecutor, 
            IProgressRecorderFactory progressRecorderFactory, 
            ILoggerFactory loggerFactory)
            : this(binaryTestExecutor, progressRecorderFactory, loggerFactory)
        {
            this.testableItems = sources.Select(source => new SourceTestableItem(source));

            this.sourceDescription = "source paths";
        }

        public MultiSourceTestExecutor(IEnumerable<TestCase> tests,
            IBinaryTestExecutor binaryTestExecutor, 
            IProgressRecorderFactory progressRecorderFactory, 
            ILoggerFactory loggerFactory)
            : this(binaryTestExecutor, progressRecorderFactory, loggerFactory)
        {
            var testCaseGroupsBySource = tests.GroupBy(t => t.Source);

            this.testableItems = testCaseGroupsBySource.Select(group => new TestCaseGroupTestableItem(group));

            this.sourceDescription = "TestCases";

            this.isCanceled = false;
        }

        MultiSourceTestExecutor(
            IBinaryTestExecutor binaryTestExecutor, 
            IProgressRecorderFactory progressRecorderFactory, 
            ILoggerFactory loggerFactory)
        {
            this.binaryTestExecutor = binaryTestExecutor;
            this.progressRecorderFactory = progressRecorderFactory;
            this.loggerFactory = loggerFactory;
        }

        public void RunTests(IFrameworkHandle frameworkHandle)
        {
            var outputLogger = loggerFactory.CreateOutput(frameworkHandle);

            outputLogger.Info(String.Format("Execution by {0} started", sourceDescription));

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

            outputLogger.Info(String.Format("Execution by {0} finished", sourceDescription));
        }

        public void CancelRun()
        {
            isCanceled = true;
        }

        bool isCanceled;

        readonly IEnumerable<ITestableItem> testableItems;
        readonly string sourceDescription;
        readonly IBinaryTestExecutor binaryTestExecutor;
        readonly IProgressRecorderFactory progressRecorderFactory;
        readonly ILoggerFactory loggerFactory;

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
    }
}
