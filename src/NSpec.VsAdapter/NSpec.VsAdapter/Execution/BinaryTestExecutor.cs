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
            IOutputLogger logger, ICrossDomainLogger crossDomainLogger)
        {
            logger.Debug(String.Format("Executing tests in container: '{0}'", binaryPath));

            BuildExecutorInvocation buildExecutorInvocation = xDomainLogger =>
                executorInvocationFactory.Create(binaryPath, progressRecorder, xDomainLogger);

            return CrossDomainExecute(binaryPath, buildExecutorInvocation, logger, crossDomainLogger);
        }

        public int Execute(string binaryPath, IEnumerable<string> testCaseFullNames, 
            IProgressRecorder progressRecorder,
            IOutputLogger logger, ICrossDomainLogger crossDomainLogger)
        {
            logger.Debug(String.Format("Executing tests in container: '{0}'", binaryPath));

            var exampleFullNames = testCaseFullNames.ToArray();

            BuildExecutorInvocation buildExecutorInvocation = xDomainLogger =>
                executorInvocationFactory.Create(binaryPath, exampleFullNames, progressRecorder, xDomainLogger);

            return CrossDomainExecute(binaryPath, buildExecutorInvocation, logger, crossDomainLogger);
        }

        int CrossDomainExecute(string binaryPath, BuildExecutorInvocation buildExecutorInvocation,
            IOutputLogger logger, ICrossDomainLogger crossDomainLogger)
        {
            int count;

            try
            {
                var executorInvocation = buildExecutorInvocation(crossDomainLogger);

                count = crossDomainExecutor.Run(binaryPath, executorInvocation.Execute);

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

        delegate IExecutorInvocation BuildExecutorInvocation(ICrossDomainLogger crossDomainLogger);
    }
}
