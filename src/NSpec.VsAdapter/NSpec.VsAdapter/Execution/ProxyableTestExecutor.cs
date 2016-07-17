using NSpec.VsAdapter.CrossDomain;
using NSpec.VsAdapter.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Execution
{
    public class ProxyableTestExecutor : Proxyable, IProxyableTestExecutor
    {
        // Cross-domain instantiation requires a default constructor
        public ProxyableTestExecutor()
        {
            this.runnableContextFinder = new RunnableContextFinder();
            this.executionReporterFactory = new ExecutionReporterFactory();
            this.contextExecutorFactory = new ContextExecutorFactory();
        }

        // Unit tests need a constructor with injected dependencies
        public ProxyableTestExecutor(
            IRunnableContextFinder runnableContextFinder,
            IExecutionReporterFactory executionReporterFactory,
            IContextExecutorFactory contextExecutorFactory)
        {
            this.runnableContextFinder = runnableContextFinder;
            this.executionReporterFactory = executionReporterFactory;
            this.contextExecutorFactory = contextExecutorFactory;
        }

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
                var runnableContexts = runnableContextFinder.Find(binaryPath, exampleFullNames);

                var executionReporter = executionReporterFactory.Create(progressRecorder);

                var contextExecutor = contextExecutorFactory.Create(executionReporter, logger);

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

        readonly IRunnableContextFinder runnableContextFinder;
        readonly IExecutionReporterFactory executionReporterFactory;
        readonly IContextExecutorFactory contextExecutorFactory;
    }
}
