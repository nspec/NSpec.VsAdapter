using NSpec.VsAdapter.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Execution
{
    public class ExecutorInvocationFactory : IExecutorInvocationFactory
    {
        // TODO pass canceler to ExecutorInvocation ctor

        public IExecutorInvocation Create(string binaryPath,
            IProgressRecorder progressRecorder, IExecutionCanceler canceler, ICrossDomainLogger logger)
        {
            return new ExecutorInvocation(binaryPath, progressRecorder, canceler, logger);
        }

        public IExecutorInvocation Create(string binaryPath, string[] exampleFullNames,
            IProgressRecorder progressRecorder, IExecutionCanceler canceler, ICrossDomainLogger logger)
        {
            return new ExecutorInvocation(binaryPath, exampleFullNames, progressRecorder, canceler, logger);
        }
    }
}
