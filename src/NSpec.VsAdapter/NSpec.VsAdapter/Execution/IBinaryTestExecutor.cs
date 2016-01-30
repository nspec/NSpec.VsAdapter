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
        int Execute(string binaryPath, 
            IProgressRecorder progressRecorder, 
            IOutputLogger outputLogger, IReplayLogger replayLogger);

        int Execute(string binaryPath, IEnumerable<string> testCaseFullNames, 
            IProgressRecorder progressRecorder,
            IOutputLogger outputLogger, IReplayLogger replayLogger);
    }
}
