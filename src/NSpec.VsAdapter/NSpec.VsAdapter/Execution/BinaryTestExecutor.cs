using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Execution
{
    public class BinaryTestExecutor : IBinaryTestExecutor
    {
        public BinaryTestExecutor(ICrossDomainOperator crossDomainOperator, IOperatorInvocationFactory operatorInvocationFactory)
        {
            this.crossDomainOperator = crossDomainOperator;
            this.operatorInvocationFactory = operatorInvocationFactory;
        }

        public void Execute(string assemblyPath, IExecutionObserver executionObserver,
            IOutputLogger logger, IReplayLogger crossDomainLogger)
        {
            logger.Debug(String.Format("Executing tests in container: '{0}'", assemblyPath));

            BuildOperatorInvocation buildOperatorInvocation = logRecorder =>
                operatorInvocationFactory.Create(assemblyPath, executionObserver, logRecorder);

            CrossDomainExecute(assemblyPath, buildOperatorInvocation, logger, crossDomainLogger);
        }

        public void Execute(string assemblyPath, IEnumerable<string> testCaseFullNames, 
            IExecutionObserver executionObserver,
            IOutputLogger logger, IReplayLogger crossDomainLogger)
        {
            logger.Debug(String.Format("Executing tests in container: '{0}'", assemblyPath));

            var exampleFullNames = testCaseFullNames.ToArray();

            BuildOperatorInvocation buildOperatorInvocation = logRecorder =>
                operatorInvocationFactory.Create(assemblyPath, exampleFullNames, executionObserver, logRecorder);

            CrossDomainExecute(assemblyPath, buildOperatorInvocation, logger, crossDomainLogger);
        }

        void CrossDomainExecute(string assemblyPath, BuildOperatorInvocation buildOperatorInvocation,
            IOutputLogger logger, IReplayLogger crossDomainLogger)
        {
            try
            {
                var logRecorder = new LogRecorder();

                var operatorInvocation = buildOperatorInvocation(logRecorder);

                int count = crossDomainOperator.Run(assemblyPath, operatorInvocation.Operate);

                var logReplayer = new LogReplayer(crossDomainLogger);

                logReplayer.Replay(logRecorder);

                logger.Debug(String.Format("Executed {0} tests", count));
            }
            catch (Exception ex)
            {
                // report problem and return for the next assembly, without crashing the test execution process

                var message = String.Format("Exception thrown while executing tests in binary '{0}'", assemblyPath);

                logger.Error(ex, message);
            }
        }

        readonly ICrossDomainOperator crossDomainOperator;
        readonly IOperatorInvocationFactory operatorInvocationFactory;

        delegate IOperatorInvocation BuildOperatorInvocation(LogRecorder logRecorder);
    }
}
