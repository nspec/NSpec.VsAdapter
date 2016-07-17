using NSpec.VsAdapter.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
