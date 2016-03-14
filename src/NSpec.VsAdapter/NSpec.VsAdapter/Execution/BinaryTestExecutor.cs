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
        public BinaryTestExecutor(IAppDomainFactory appDomainFactory, IProxyableFactory<IProxyableTestExecutor> proxyableFactory)
        {
            this.appDomainFactory = appDomainFactory;
            this.proxyableFactory = proxyableFactory;
        }

        public int ExecuteAll(string binaryPath, IProgressRecorder progressRecorder,
            IOutputLogger logger, ICrossDomainLogger crossDomainLogger)
        {
            RemoteOperation operation = (proxyableExecutor) =>
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

            RemoteOperation operation = (proxyableExecutor) =>
            {
                return proxyableExecutor.ExecuteSelection(binaryPath, exampleFullNames, progressRecorder, crossDomainLogger);
            };

            return RunRemoteOperation("selected", operation, binaryPath, logger);
        }

        // TODO pass canceler to proxyableExecutor

        int RunRemoteOperation(string description, RemoteOperation operation, string binaryPath, IOutputLogger logger)
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
        readonly IProxyableFactory<IProxyableTestExecutor> proxyableFactory;

        delegate int RemoteOperation(IProxyableTestExecutor proxyableTestExecutor);
    }
}
