using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Execution
{
    public interface IOperatorInvocationFactory
    {
        IOperatorInvocation Create(string binaryPath, 
            IExecutionObserver executionObserver, LogRecorder logRecorder);

        IOperatorInvocation Create(string binaryPath, string[] exampleFullNames, 
            IExecutionObserver executionObserver, LogRecorder logRecorder);
    }
}
