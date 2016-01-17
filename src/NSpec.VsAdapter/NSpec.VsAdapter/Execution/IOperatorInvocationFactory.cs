using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Execution
{
    public interface IOperatorInvocationFactory
    {
        IOperatorInvocation Create(string assemblyPath, 
            IExecutionObserver executionObserver, LogRecorder logRecorder);

        IOperatorInvocation Create(string assemblyPath, string[] exampleFullNames, 
            IExecutionObserver executionObserver, LogRecorder logRecorder);
    }
}
