using NSpec.VsAdapter.CrossDomain;
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
        public BinaryTestExecutor(IAppDomainFactory appDomainFactory, IProxyableFactory<ProxyableTestExecutor> proxyableFactory)
        {
            this.appDomainFactory = appDomainFactory;
            this.proxyableFactory = proxyableFactory;
        }

        // TODO build some ExecutionContext or ExecutionScenario, or one of two descendants: 
        // AllScenario and SelectedScenario. Any of them would store all input parameters
        // and return/perform its own specific "operation"

        public int Execute(string binaryPath, IProgressRecorder progressRecorder,
            IOutputLogger logger, ICrossDomainLogger crossDomainLogger)
        {
            Func<ProxyableTestExecutor, int> operation = (proxyableExecutor) =>
            {
                return proxyableExecutor.ExecuteAll(binaryPath, progressRecorder, crossDomainLogger);
            };

            return ExecuteScenario("all", operation, binaryPath, logger);
        }

        public int Execute(string binaryPath, IEnumerable<string> testCaseFullNames, 
            IProgressRecorder progressRecorder,
            IOutputLogger logger, ICrossDomainLogger crossDomainLogger)
        {
            string[] exampleFullNames = testCaseFullNames.ToArray();

            Func<ProxyableTestExecutor, int> operation = (proxyableExecutor) =>
            {
                return proxyableExecutor.ExecuteSelection(binaryPath, exampleFullNames, progressRecorder, crossDomainLogger);
            };

            return ExecuteScenario("selected", operation, binaryPath, logger);
        }

        // TODO pass canceler to proxyableExecutor

        int ExecuteScenario(string description, Func<ProxyableTestExecutor, int> operation, string binaryPath, IOutputLogger logger)
        {
            logger.Info(String.Format("Executing {0} tests in binary '{1}'", description, binaryPath));

            int count;

            try
            {
                using (var targetDomain = appDomainFactory.Create(binaryPath))
                using (var proxyableTestExecutor = proxyableFactory.CreateProxy(targetDomain))
                {
                    count = operation(proxyableTestExecutor);
                }
            }
            catch (Exception ex)
            {
                count = 0;

                // report problem and return for the next assembly, without crashing the test execution process
                var message = String.Format("Exception thrown while executing tests in binary '{0}'", binaryPath);
                logger.Error(ex, message);
            }

            logger.Info(String.Format("Executed {0} tests in binary '{1}'", count, binaryPath));

            return count;
        }

        readonly IAppDomainFactory appDomainFactory;
        readonly IProxyableFactory<ProxyableTestExecutor> proxyableFactory;
    }
}
