using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Execution
{
    public interface ICrossDomainTestExecutor
    {
        void Execute(string assemblyPath, 
            IExecutionObserver executionObserver, 
            IOutputLogger outputLogger, IReplayLogger replayLogger);

        void Execute(string assemblyPath, IEnumerable<string> testCaseFullNames, 
            IExecutionObserver executionObserver,
            IOutputLogger outputLogger, IReplayLogger replayLogger);
    }
}
