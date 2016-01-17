using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Execution
{
    public class OperatorInvocationFactory : IOperatorInvocationFactory
    {
        public IOperatorInvocation Create(string assemblyPath, 
            IExecutionObserver executionObserver, LogRecorder logRecorder)
        {
            return new OperatorInvocation(assemblyPath, executionObserver, logRecorder);
        }

        public IOperatorInvocation Create(string assemblyPath, string[] exampleFullNames, 
            IExecutionObserver executionObserver, LogRecorder logRecorder)
        {
            return new OperatorInvocation(assemblyPath, exampleFullNames, executionObserver, logRecorder);
        }
    }
}
