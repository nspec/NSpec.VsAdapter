using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using NSpec.Domain;
using NSpec.VsAdapter.TestAdapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;

namespace NSpec.VsAdapter.Execution
{
    public class ProgressRecorder : MarshalByRefObject, IProgressRecorder
    {
        public ProgressRecorder(ITestExecutionRecorder testExecutionRecorder, ITestResultMapper testResultMapper)
        {
            this.testExecutionRecorder = testExecutionRecorder;
            this.testResultMapper = testResultMapper;
        }

        // MarshalByRefObject

        public override object InitializeLifetimeService()
        {
            // Claim an infinite lease lifetime by returning null here. 
            // To prevent memory leaks as a side effect, instance creators 
            // *must* Dispose() in order to explicitly end the lifetime.

            return null;
        }

        // IProgressRecorder

        public string BinaryPath { private get; set; }

        public void RecordExecutedExample(ExecutedExample executedExample)
        {
            var testResult = testResultMapper.FromExecutedExample(executedExample, BinaryPath);

            testExecutionRecorder.RecordResult(testResult);
        }

        public virtual void Dispose()
        {
            RemotingServices.Disconnect(this);
        }

        readonly ITestExecutionRecorder testExecutionRecorder;
        readonly ITestResultMapper testResultMapper;
    }
}
