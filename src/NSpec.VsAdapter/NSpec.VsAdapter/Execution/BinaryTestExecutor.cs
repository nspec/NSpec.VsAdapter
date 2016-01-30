using NSpec.VsAdapter.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Execution
{
    public class BinaryTestExecutor : IBinaryTestExecutor
    {
        public BinaryTestExecutor(ICrossDomainExecutor crossDomainExecutor, IExecutorInvocationFactory executorInvocationFactory)
        {
            this.crossDomainExecutor = crossDomainExecutor;
            this.executorInvocationFactory = executorInvocationFactory;
        }

        public int Execute(string binaryPath, IProgressRecorder progressRecorder,
            IOutputLogger logger, IReplayLogger replayLogger)
        {
            logger.Debug(String.Format("Executing tests in container: '{0}'", binaryPath));

            BuildExecutorInvocation buildExecutorInvocation = logRecorder =>
                executorInvocationFactory.Create(binaryPath, progressRecorder, logRecorder);

            return CrossDomainExecute(binaryPath, buildExecutorInvocation, logger, replayLogger);
        }

        public int Execute(string binaryPath, IEnumerable<string> testCaseFullNames, 
            IProgressRecorder progressRecorder,
            IOutputLogger logger, IReplayLogger replayLogger)
        {
            logger.Debug(String.Format("Executing tests in container: '{0}'", binaryPath));

            var exampleFullNames = testCaseFullNames.ToArray();

            BuildExecutorInvocation buildExecutorInvocation = logRecorder =>
                executorInvocationFactory.Create(binaryPath, exampleFullNames, progressRecorder, logRecorder);

            return CrossDomainExecute(binaryPath, buildExecutorInvocation, logger, replayLogger);
        }

        int CrossDomainExecute(string binaryPath, BuildExecutorInvocation buildExecutorInvocation,
            IOutputLogger logger, IReplayLogger replayLogger)
        {
            int count;

            try
            {
                var logRecorder = new LogRecorder();

                var executorInvocation = buildExecutorInvocation(logRecorder);

                count = crossDomainExecutor.Run(binaryPath, executorInvocation.Execute);

                var logReplayer = new LogReplayer(replayLogger);

                logReplayer.Replay(logRecorder);

                logger.Debug(String.Format("Executed {0} tests", count));
            }
            catch (Exception ex)
            {
                // report problem and return for the next assembly, without crashing the test execution process

                var message = String.Format("Exception thrown while executing tests in binary '{0}'", binaryPath);

                logger.Error(ex, message);

                count = 0;
            }

            return count;
        }

        readonly ICrossDomainExecutor crossDomainExecutor;
        readonly IExecutorInvocationFactory executorInvocationFactory;

        delegate IExecutorInvocation BuildExecutorInvocation(LogRecorder logRecorder);
    }
}
