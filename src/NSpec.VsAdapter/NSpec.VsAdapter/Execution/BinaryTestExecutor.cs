using NSpec.VsAdapter.CrossDomain;
using NSpec.VsAdapter.Execution.Target;
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
        public BinaryTestExecutor(ICrossDomainRunner<IProxyableTestExecutor, int> remoteRunner)
        {
            this.remoteRunner = remoteRunner;
        }

        public int ExecuteAll(string binaryPath, IProgressRecorder progressRecorder,
            IOutputLogger logger, ICrossDomainLogger crossDomainLogger)
        {
            Func<IProxyableTestExecutor, int> operation = (proxyableExecutor) =>
            {
                return proxyableExecutor.ExecuteAll(binaryPath, progressRecorder, crossDomainLogger);
            };

            return RunRemoteOperation("all", operation, binaryPath, logger);
        }

        public int ExecuteSelected(string binaryPath, IEnumerable<string> testCaseFullNames, 
            IProgressRecorder progressRecorder,
            IOutputLogger logger, ICrossDomainLogger crossDomainLogger)
        {
            string[] exampleFullNames = testCaseFullNames.ToArray();

            Func<IProxyableTestExecutor, int> operation = (proxyableExecutor) =>
            {
                return proxyableExecutor.ExecuteSelection(binaryPath, exampleFullNames, progressRecorder, crossDomainLogger);
            };

            return RunRemoteOperation("selected", operation, binaryPath, logger);
        }

        // TODO pass canceler to proxyableExecutor

        int RunRemoteOperation(string description, Func<IProxyableTestExecutor, int> operation, string binaryPath, IOutputLogger logger)
        {
            logger.Info(String.Format("Executing {0} tests in binary '{1}'", description, binaryPath));

            int count = remoteRunner.Run(binaryPath, operation, (ex, path) =>
            {
                // report problem and return for the next assembly, without crashing the test execution process
                var message = String.Format("Exception thrown while executing tests in binary '{0}'", path);
                logger.Error(ex, message);

                return 0;
            });

            logger.Info(String.Format("Executed {0} tests in binary '{1}'", count, binaryPath));

            return count;
        }

        readonly ICrossDomainRunner<IProxyableTestExecutor, int> remoteRunner;
    }
}
