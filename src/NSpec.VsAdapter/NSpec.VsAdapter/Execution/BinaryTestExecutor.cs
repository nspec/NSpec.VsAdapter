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

        public int Execute(string binaryPath,
            IProgressRecorder progressRecorder, IExecutionCanceler canceler,
            IOutputLogger logger, ICrossDomainLogger crossDomainLogger)
        {
            logger.Info(String.Format("Executing all tests in binary '{0}'", binaryPath));

            var executorInvocation = executorInvocationFactory.Create(binaryPath, progressRecorder, canceler, crossDomainLogger);

            return CrossDomainExecute(binaryPath, executorInvocation, logger);
        }

        public int Execute(string binaryPath, IEnumerable<string> testCaseFullNames,
            IProgressRecorder progressRecorder, IExecutionCanceler canceler,
            IOutputLogger logger, ICrossDomainLogger crossDomainLogger)
        {
            logger.Info(String.Format("Executing selected tests in binary '{0}'", binaryPath));

            var exampleFullNames = testCaseFullNames.ToArray();

            var executorInvocation = executorInvocationFactory.Create(binaryPath, exampleFullNames, progressRecorder, canceler, crossDomainLogger);

            return CrossDomainExecute(binaryPath, executorInvocation, logger);
        }

        int CrossDomainExecute(string binaryPath, IExecutorInvocation executorInvocation,
            IOutputLogger logger)
        {
            int count;

            try
            {
                count = crossDomainExecutor.Run(binaryPath, executorInvocation.Execute);

                logger.Info(String.Format("Executed {0} tests", count));
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
    }
}
