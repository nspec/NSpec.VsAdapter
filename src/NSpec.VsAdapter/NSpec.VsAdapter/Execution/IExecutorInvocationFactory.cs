using NSpec.VsAdapter.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Execution
{
    public interface IExecutorInvocationFactory
    {
        IExecutorInvocation Create(string binaryPath, 
            IProgressRecorder progressRecorder, ICrossDomainLogger logger);

        IExecutorInvocation Create(string binaryPath, string[] exampleFullNames, 
            IProgressRecorder progressRecorder, ICrossDomainLogger logger);
    }
}
