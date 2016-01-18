using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Execution
{
    public class BinaryTestExecutor : IBinaryTestExecutor
    {
        public BinaryTestExecutor(ICrossDomainExecutor crossDomainExecutor, IOperatorInvocationFactory operatorInvocationFactory)
        {
            this.crossDomainExecutor = crossDomainExecutor;
            this.operatorInvocationFactory = operatorInvocationFactory;
        }

        public void Execute(string binaryPath, IExecutionObserver executionObserver,
            IOutputLogger logger, IReplayLogger crossDomainLogger)
        {
            logger.Debug(String.Format("Executing tests in container: '{0}'", binaryPath));

            BuildOperatorInvocation buildOperatorInvocation = logRecorder =>
                operatorInvocationFactory.Create(binaryPath, executionObserver, logRecorder);

            CrossDomainExecute(binaryPath, buildOperatorInvocation, logger, crossDomainLogger);
        }

        public void Execute(string binaryPath, IEnumerable<string> testCaseFullNames, 
            IExecutionObserver executionObserver,
            IOutputLogger logger, IReplayLogger crossDomainLogger)
        {
            logger.Debug(String.Format("Executing tests in container: '{0}'", binaryPath));

            var exampleFullNames = testCaseFullNames.ToArray();

            BuildOperatorInvocation buildOperatorInvocation = logRecorder =>
                operatorInvocationFactory.Create(binaryPath, exampleFullNames, executionObserver, logRecorder);

            CrossDomainExecute(binaryPath, buildOperatorInvocation, logger, crossDomainLogger);
        }

        void CrossDomainExecute(string binaryPath, BuildOperatorInvocation buildOperatorInvocation,
            IOutputLogger logger, IReplayLogger crossDomainLogger)
        {
            try
            {
                var logRecorder = new LogRecorder();

                var operatorInvocation = buildOperatorInvocation(logRecorder);

                int count = crossDomainExecutor.Run(binaryPath, operatorInvocation.Operate);

                var logReplayer = new LogReplayer(crossDomainLogger);

                logReplayer.Replay(logRecorder);

                logger.Debug(String.Format("Executed {0} tests", count));
            }
            catch (Exception ex)
            {
                // report problem and return for the next assembly, without crashing the test execution process

                var message = String.Format("Exception thrown while executing tests in binary '{0}'", binaryPath);

                logger.Error(ex, message);
            }
        }

        readonly ICrossDomainExecutor crossDomainExecutor;
        readonly IOperatorInvocationFactory operatorInvocationFactory;

        delegate IOperatorInvocation BuildOperatorInvocation(LogRecorder logRecorder);
    }
}
