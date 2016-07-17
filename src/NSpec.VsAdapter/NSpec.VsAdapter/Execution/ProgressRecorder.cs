using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using NSpec.VsAdapter.CrossDomain;
using NSpec.VsAdapter.TestAdapter;

namespace NSpec.VsAdapter.Execution
{
    public class ProgressRecorder : Proxyable, IProgressRecorder
    {
        public ProgressRecorder(ITestExecutionRecorder testExecutionRecorder, ITestResultMapper testResultMapper)
        {
            this.testExecutionRecorder = testExecutionRecorder;
            this.testResultMapper = testResultMapper;
        }

        // IProgressRecorder

        public string BinaryPath { private get; set; }

        public void RecordExecutedExample(ExecutedExample executedExample)
        {
            var testResult = testResultMapper.FromExecutedExample(executedExample, BinaryPath);

            testExecutionRecorder.RecordResult(testResult);
        }

        readonly ITestExecutionRecorder testExecutionRecorder;
        readonly ITestResultMapper testResultMapper;
    }
}
