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
            IProgressRecorder progressRecorder, LogRecorder logRecorder)
        {
            return new ExecutorInvocation(binaryPath, progressRecorder, logRecorder);
        }

        public IExecutorInvocation Create(string binaryPath, string[] exampleFullNames, 
            IProgressRecorder progressRecorder, LogRecorder logRecorder)
        {
            return new ExecutorInvocation(binaryPath, exampleFullNames, progressRecorder, logRecorder);
        }
    }
}
