using NSpec.VsAdapter.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Execution
{
    public interface IProxyableTestExecutor
    {
        int ExecuteAll(string binaryPath, 
            IProgressRecorder progressRecorder, ICrossDomainLogger logger);

        int ExecuteSelection(string binaryPath, string[] exampleFullNames, 
            IProgressRecorder progressRecorder, ICrossDomainLogger logger);
    }
}
