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
            CrossDomainRunner<IProxyableTestExecutor, int>.RemoteOperation operation = (proxyableExecutor) =>
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

            CrossDomainRunner<IProxyableTestExecutor, int>.RemoteOperation operation = (proxyableExecutor) =>
            {
                return proxyableExecutor.ExecuteSelection(binaryPath, exampleFullNames, progressRecorder, crossDomainLogger);
            };

            return RunRemoteOperation("selected", operation, binaryPath, logger);
        }

        // TODO pass canceler to proxyableExecutor

        int RunRemoteOperation(string description, CrossDomainRunner<IProxyableTestExecutor, int>.RemoteOperation operation, string binaryPath, IOutputLogger logger)
        {
            logger.Info(String.Format("Executing {0} tests in binary '{1}'", description, binaryPath));

            var remoteRunner = new CrossDomainRunner<IProxyableTestExecutor, int>(appDomainFactory, proxyableFactory);

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

        readonly IAppDomainFactory appDomainFactory;
        readonly IProxyableFactory<IProxyableTestExecutor> proxyableFactory;
    }
}
