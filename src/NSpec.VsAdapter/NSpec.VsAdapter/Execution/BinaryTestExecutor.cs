using NSpec.VsAdapter.CrossDomain;
using NSpec.VsAdapter.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

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
            var executionOperation = new BinaryExecutionOperation(binaryPath,
                progressRecorder, crossDomainLogger);

            return RunOperationRemotely("all", executionOperation, binaryPath, logger);
        }

        public int ExecuteSelected(string binaryPath, IEnumerable<string> testCaseFullNames,
            IProgressRecorder progressRecorder,
            IOutputLogger logger, ICrossDomainLogger crossDomainLogger)
        {
            string[] exampleFullNames = testCaseFullNames.ToArray();

            var executionOperation = new SelectionExecutionOperation(binaryPath, exampleFullNames,
                progressRecorder, crossDomainLogger);

            return RunOperationRemotely("selected", executionOperation, binaryPath, logger);
        }

        // TODO pass canceler to proxyableExecutor

        int RunOperationRemotely(string description,
            IExecutionOperation executionOperation, string binaryPath, IOutputLogger logger)
        {
            logger.Info(String.Format("Executing {0} tests in binary '{1}'", description, binaryPath));

            int count = remoteRunner.Run(binaryPath, executionOperation.Run, (ex, path) =>
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

        interface IExecutionOperation
        {
            int Run(IProxyableTestExecutor proxyableExecutor);
        }

        class BinaryExecutionOperation : IExecutionOperation
        {
            public BinaryExecutionOperation(string binaryPath,
                IProgressRecorder progressRecorder, ICrossDomainLogger crossDomainLogger)
            {
                this.binaryPath = binaryPath;
                this.progressRecorder = progressRecorder;
                this.crossDomainLogger = crossDomainLogger;
            }

            public virtual int Run(IProxyableTestExecutor proxyableExecutor)
            {
                return proxyableExecutor.ExecuteAll(binaryPath, progressRecorder, crossDomainLogger);
            }

            protected readonly string binaryPath;
            protected readonly IProgressRecorder progressRecorder;
            protected readonly ICrossDomainLogger crossDomainLogger;
        }

        class SelectionExecutionOperation : BinaryExecutionOperation
        {
            public SelectionExecutionOperation(string binaryPath, string[] exampleFullNames,
                IProgressRecorder progressRecorder, ICrossDomainLogger crossDomainLogger)
                : base(binaryPath, progressRecorder, crossDomainLogger)
            {
                this.exampleFullNames = exampleFullNames;
            }

            public override int Run(IProxyableTestExecutor proxyableExecutor)
            {
                // do *not* call base implementation

                return proxyableExecutor.ExecuteSelection(binaryPath, exampleFullNames, progressRecorder, crossDomainLogger);
            }

            readonly string[] exampleFullNames;
        }
    }
}
