using NSpec.Domain;
using NSpec.Domain.Formatters;
using NSpec.VsAdapter.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSpec.VsAdapter.Core.Execution.Target
{
    // TODO add unit tests

    public class ContextExecutor : IContextExecutor
    {
        public ContextExecutor(ILiveFormatter executionReporter, ICrossDomainLogger logger)
        {
            this.executionReporter = executionReporter;
            this.logger = logger;
        }

        public int Execute(IEnumerable<RunnableContext> runnableContexts)
        {
            // TODO implement execution canceling

            int totalExamplecount = 0;

            foreach (var runnableContext in runnableContexts)
            {
                logger.Debug(String.Format("Start executing tests in context '{0}'", runnableContext.Name));

                runnableContext.Run(executionReporter);

                int contextExampleCount = runnableContext.ExampleCount;

                logger.Debug(String.Format("Done executing {0} tests in context '{1}'", contextExampleCount, runnableContext.Name));

                totalExamplecount += contextExampleCount;
            }

            return totalExamplecount;
        }

        readonly ILiveFormatter executionReporter;
        readonly ICrossDomainLogger logger;
    }
}
