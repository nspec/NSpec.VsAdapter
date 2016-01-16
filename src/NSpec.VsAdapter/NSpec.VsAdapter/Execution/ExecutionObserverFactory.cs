using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Execution
{
    public class ExecutionObserverFactory : IExecutionObserverFactory
    {
        public IExecutionObserver Create(ITestExecutionRecorder testExecutionRecorder)
        {
            return new ExecutionObserver(testExecutionRecorder);
        }
    }
}
