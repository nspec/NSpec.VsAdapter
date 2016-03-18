using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using NSpec.Domain;
using NSpec.VsAdapter.Core.CrossDomain;
using NSpec.VsAdapter.TestAdapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;

namespace NSpec.VsAdapter.Core.Execution
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
