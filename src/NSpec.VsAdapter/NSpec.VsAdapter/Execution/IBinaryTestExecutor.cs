using NSpec.VsAdapter.Logging;
using System.Collections.Generic;

namespace NSpec.VsAdapter.Execution
{
    public interface IBinaryTestExecutor
    {
        int ExecuteAll(string binaryPath, 
            IProgressRecorder progressRecorder, 
            IOutputLogger outputLogger, ICrossDomainLogger crossDomainLogger);

        int ExecuteSelected(string binaryPath, IEnumerable<string> testCaseFullNames, 
            IProgressRecorder progressRecorder,
            IOutputLogger outputLogger, ICrossDomainLogger crossDomainLogger);
    }
}
