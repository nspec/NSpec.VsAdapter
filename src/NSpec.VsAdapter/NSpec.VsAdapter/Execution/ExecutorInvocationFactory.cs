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
            IProgressRecorder progressRecorder, ICrossDomainLogger logger)
        {
            return new ExecutorInvocation(binaryPath, progressRecorder, logger);
        }

        public IExecutorInvocation Create(string binaryPath, string[] exampleFullNames, 
            IProgressRecorder progressRecorder, ICrossDomainLogger logger)
        {
            return new ExecutorInvocation(binaryPath, exampleFullNames, progressRecorder, logger);
        }
    }
}
