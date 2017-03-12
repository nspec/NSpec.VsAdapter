using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.IntegrationTests.Execution
{
    [Serializable]
    public class CollectingFrameworkHandle : IFrameworkHandle
    {
        public CollectingFrameworkHandle(ConsoleLogger consoleLogger)
        {
            StartedTestCases = new List<TestCase>();
            EndedTestInfo = new List<Tuple<TestCase, TestOutcome>>();
            Results = new List<TestResult>();

            this.consoleLogger = consoleLogger;
        }

        public List<TestCase> StartedTestCases { get; private set; }

        public List<Tuple<TestCase, TestOutcome>> EndedTestInfo { get; private set; }

        public List<TestResult> Results { get; private set; }

        public void RecordStart(TestCase testCase)
        {
            StartedTestCases.Add(testCase);
        }

        public void RecordEnd(TestCase testCase, TestOutcome outcome)
        {
            EndedTestInfo.Add(new Tuple<TestCase, TestOutcome>(testCase, outcome));
        }

        public void RecordResult(TestResult testResult)
        {
            Results.Add(testResult);
        }

        public void RecordAttachments(IList<AttachmentSet> attachmentSets)
        {
            throw new DummyTestException();
        }

        public int LaunchProcessWithDebuggerAttached(string filePath, string workingDirectory, string arguments, IDictionary<string, string> environmentVariables)
        {
            throw new DummyTestException();
        }

        public bool EnableShutdownAfterTestRun { get; set; }

        public void SendMessage(TestMessageLevel testMessageLevel, string message)
        {
            consoleLogger.SendMessage(testMessageLevel, message);
        }

        readonly ConsoleLogger consoleLogger;
    }
}
