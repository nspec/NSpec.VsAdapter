using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using NSpec.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSpec.VsAdapter.Execution
{
    public class ExecutionObserver : IExecutionObserver
    {
        public ExecutionObserver(ITestExecutionRecorder testExecutionRecorder)
        {
            this.testExecutionRecorder = testExecutionRecorder;
        }

        public void Write(ExampleBase example, int level)
        {
            throw new NotImplementedException();
        }

        public void Write(Context context)
        {
            throw new NotImplementedException();
        }

        readonly ITestExecutionRecorder testExecutionRecorder;
    }
}
