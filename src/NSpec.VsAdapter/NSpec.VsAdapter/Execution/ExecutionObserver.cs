using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using NSpec.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSpec.VsAdapter.Execution
{
    [Serializable]
    public class ExecutionObserver : IExecutionObserver
    {
        public ExecutionObserver(ITestExecutionRecorder testExecutionRecorder, ITestResultMapper testResultMapper)
        {
            this.testExecutionRecorder = testExecutionRecorder;
            this.testResultMapper = testResultMapper;
        }

        public string BinaryPath { private get; set; }

        public void Write(ExampleBase example, int level)
        {
            // ignore level

            var testResult = testResultMapper.FromExample(example, BinaryPath);

            testExecutionRecorder.RecordResult(testResult);
        }

        public void Write(Context context)
        {
            // do nothing
        }

        readonly ITestExecutionRecorder testExecutionRecorder;
        readonly ITestResultMapper testResultMapper;
    }
}
