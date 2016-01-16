using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Execution
{
    public class CrossDomainTestExecutor : ICrossDomainTestExecutor
    {
        public CrossDomainTestExecutor(ICrossDomainOperator crossDomainOperator)
        {
            this.crossDomainOperator = crossDomainOperator;
        }

        public void Execute(string assemblyPath, IExecutionObserver executionObserver,
            IOutputLogger logger, IReplayLogger crossDomainLogger)
        {
            int count;

            try
            {
                logger.Debug(String.Format("Executing tests in container: '{0}'", assemblyPath));

                var logRecorder = new LogRecorder();

                var operatorInvocation = new OperatorInvocation(assemblyPath, executionObserver, logRecorder);

                count = crossDomainOperator.Run(assemblyPath, operatorInvocation.Operate);

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

        public void Execute(string assemblyPath, IEnumerable<string> testCaseFullNames, 
            IExecutionObserver executionObserver,
            IOutputLogger logger, IReplayLogger crossDomainLogger)
        {
            // TODO extract common logic in two Execute() methods

            int count;

            try
            {
                logger.Debug(String.Format("Executing tests in container: '{0}'", assemblyPath));

                var logRecorder = new LogRecorder();

                var exampleFullNames = testCaseFullNames.ToArray();

                var operatorInvocation = new OperatorInvocation(assemblyPath, exampleFullNames, executionObserver, logRecorder);

                count = crossDomainOperator.Run(assemblyPath, operatorInvocation.Operate);

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
    }
}
