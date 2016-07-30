using NSpec.VsAdapter.Core.CrossDomain;
using NSpec.VsAdapter.Logging;
using System;

namespace NSpec.VsAdapter.Core.Execution.Target
{
    public class ProxyableTestExecutor : Proxyable, IProxyableTestExecutor
    {
        public int ExecuteAll(string binaryPath, 
            IProgressRecorder progressRecorder, ICrossDomainLogger logger)
        {
            return Execute(binaryPath, RunnableContextFinder.RunAll, progressRecorder, logger);
        }

        public int ExecuteSelection(string binaryPath, string[] exampleFullNames, 
            IProgressRecorder progressRecorder, ICrossDomainLogger logger)
        {
            return Execute(binaryPath, exampleFullNames, progressRecorder, logger);
        }

        int Execute(string binaryPath, string[] exampleFullNames,
            IProgressRecorder progressRecorder, ICrossDomainLogger logger)
        {
            string scenario = (exampleFullNames == RunnableContextFinder.RunAll ? "all" : "selected");

            logger.Debug(String.Format("Start executing {0} tests locally in binary '{1}'", scenario, binaryPath));

            int count;

            try
            {
                var runnableContextFinder = new RunnableContextFinder();

                var runnableContexts = runnableContextFinder.Find(binaryPath, exampleFullNames);

                var executedExampleMapper = new ExecutedExampleMapper();

                var executionReporter = new ExecutionReporter(progressRecorder, executedExampleMapper);

                var contextExecutor = new ContextExecutor(executionReporter, logger);

                count = contextExecutor.Execute(runnableContexts);
            }
            catch (Exception ex)
            {
                // report problem and return, without letting exception cross app domain boundary

                count = 0;

                var exInfo = new ExceptionLogInfo(ex);
                var message = String.Format("Exception thrown while executing tests locally in binary '{0}'", binaryPath);

                logger.Error(exInfo, message);
            }

            logger.Debug(String.Format("Finish executing {0} tests locally in binary '{1}'", count, binaryPath));

            return count;
        }
    }
}
