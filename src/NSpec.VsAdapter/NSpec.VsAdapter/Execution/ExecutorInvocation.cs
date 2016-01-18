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
            : this(binaryPath, runAll, executionObserver, logger)
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

        public int Operate()
        {
            logger.Debug(String.Format("Start operating tests in '{0}'", binaryPath));

            var contextFinder = new ContextFinder();

            var contexts = contextFinder.BuildContexts(binaryPath);

            IEnumerable<ExampleBase> ranExamples;

            if (exampleFullNames == runAll)
            {
                contexts.Run(executionObserver, false);

                ranExamples = contexts.SelectMany(ctx => ctx.Examples);
            }
            else
            {
                // original idea taken from https://github.com/osoftware/NSpecTestAdapter/blob/master/NSpec.TestAdapter/Executor.cs

                var allExamples = contexts.SelectMany(ctx => ctx.Examples);

                var selectedNames = new HashSet<string>(exampleFullNames);

                var selectedExamples = allExamples.Where(exm => selectedNames.Contains(exm.FullName()));

                var selectedContexts = selectedExamples.Select(exm => exm.Context);

                foreach (var context in selectedContexts)
                {
                    context.Run(executionObserver, false);
                }

                ranExamples = selectedContexts.SelectMany(ctx => ctx.Examples);
            }

            int count = ranExamples.Count();

            logger.Debug(String.Format("Finish operating tests in '{0}'", binaryPath));

            logger.Flush();

            return count;
        }

        readonly string binaryPath;
        readonly string[] exampleFullNames;
        readonly IExecutionObserver executionObserver;
        readonly ISerializableLogger logger;

        static readonly string[] runAll = null;
    }
}
