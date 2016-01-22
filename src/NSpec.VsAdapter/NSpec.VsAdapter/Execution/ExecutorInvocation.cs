using NSpec.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSpec.VsAdapter.Execution
{
    [Serializable]
    public class ExecutorInvocation : IExecutorInvocation
    {
        public ExecutorInvocation(string binaryPath, 
            IProgressRecorder progressRecorder, ISerializableLogger logger)
            : this(binaryPath, RunnableContextFinder.RunAll, progressRecorder, logger)
        {
        }

        public ExecutorInvocation(string binaryPath, string[] exampleFullNames, 
            IProgressRecorder progressRecorder, ISerializableLogger logger)
        {
            this.binaryPath = binaryPath;
            this.exampleFullNames = exampleFullNames;
            this.progressRecorder = progressRecorder;
            this.logger = logger;
        }

        public int Execute()
        {
            logger.Debug(String.Format("Start executing tests in '{0}'", binaryPath));

            var runnableContextFinder = new RunnableContextFinder();

            var runnableContexts = runnableContextFinder.Find(binaryPath, exampleFullNames);

            var executedExampleMapper = new ExecutedExampleMapper();

            var executionReporter = new ExecutionReporter(progressRecorder, executedExampleMapper);

            var contextExecutor = new ContextExecutor(executionReporter);

            int count = contextExecutor.Execute(runnableContexts);

            logger.Debug(String.Format("Finish executing tests in '{0}'", binaryPath));

            logger.Flush();

            return count;
        }

        readonly string binaryPath;
        readonly string[] exampleFullNames;
        readonly IProgressRecorder progressRecorder;
        readonly ISerializableLogger logger;
    }
}
