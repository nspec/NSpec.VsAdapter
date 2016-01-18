using NSpec.Domain;
using NSpec.VsAdapter.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSpec.VsAdapter.Execution
{
    public class ExecutorInvocation : IExecutorInvocation
    {
        public ExecutorInvocation(string binaryPath, 
            IExecutionObserver executionObserver, ISerializableLogger logger)
            : this(binaryPath, RunnableContextFinder.RunAll, executionObserver, logger)
        {
        }

        public ExecutorInvocation(string binaryPath, string[] exampleFullNames, 
            IExecutionObserver executionObserver, ISerializableLogger logger)
        {
            this.binaryPath = binaryPath;
            this.exampleFullNames = exampleFullNames;
            this.executionObserver = executionObserver;
            this.logger = logger;
        }

        public int Execute()
        {
            logger.Debug(String.Format("Start executing tests in '{0}'", binaryPath));

            var runnableContextFinder = new RunnableContextFinder();

            var runnableContexts = runnableContextFinder.Find(binaryPath, exampleFullNames);

            foreach (var context in runnableContexts)
            {
                context.Run(executionObserver, false);
            }

            var ranExamples = runnableContexts.SelectMany(ctx => ctx.Examples);

            int count = ranExamples.Count();

            logger.Debug(String.Format("Finish executing tests in '{0}'", binaryPath));

            logger.Flush();

            return count;
        }

        readonly string binaryPath;
        readonly string[] exampleFullNames;
        readonly IExecutionObserver executionObserver;
        readonly ISerializableLogger logger;
    }
}
