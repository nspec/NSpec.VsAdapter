using NSpec.Domain;
using NSpec.Domain.Formatters;
using NSpec.VsAdapter.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSpec.VsAdapter.Execution
{
    public class ContextExecutor
    {
        public ContextExecutor(ILiveFormatter executionReporter, IExecutionCanceler canceler, ICrossDomainLogger logger)
        {
            this.executionReporter = executionReporter;
            this.canceler = canceler;
            this.logger = logger;
        }

        public int Execute(IEnumerable<IRunnableContext> runnableContexts)
        {
            int totalExamplecount = 0;

            foreach (var runnableContext in runnableContexts)
            {
                if (canceler.IsCanceled)
                {
                    break;
                }

                logger.Debug(String.Format("Start executing tests in context '{0}'", runnableContext.Name));

                runnableContext.Run(executionReporter);

                int contextExampleCount = runnableContext.ExampleCount;

                logger.Debug(String.Format("Done executing {0} tests in context '{1}'", contextExampleCount, runnableContext.Name));

                totalExamplecount += contextExampleCount;
            }

            return totalExamplecount;
        }

        readonly ILiveFormatter executionReporter;
        readonly IExecutionCanceler canceler;
        readonly ICrossDomainLogger logger;
    }
}
