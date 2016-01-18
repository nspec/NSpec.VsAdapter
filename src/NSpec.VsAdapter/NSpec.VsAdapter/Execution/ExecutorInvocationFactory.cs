using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Execution
{
    public class ExecutorInvocationFactory : IExecutorInvocationFactory
    {
        public IExecutorInvocation Create(string binaryPath, 
            IExecutionObserver executionObserver, LogRecorder logRecorder)
        {
            return new ExecutorInvocation(binaryPath, executionObserver, logRecorder);
        }

        public IExecutorInvocation Create(string binaryPath, string[] exampleFullNames, 
            IExecutionObserver executionObserver, LogRecorder logRecorder)
        {
            return new ExecutorInvocation(binaryPath, exampleFullNames, executionObserver, logRecorder);
        }
    }
}
