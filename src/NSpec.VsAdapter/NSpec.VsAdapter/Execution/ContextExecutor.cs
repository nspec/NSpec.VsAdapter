using NSpec.Domain;
using NSpec.Domain.Formatters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSpec.VsAdapter.Execution
{
    public class ContextExecutor
    {
        public ContextExecutor(ILiveFormatter executionReporter)
        {
            this.executionReporter = executionReporter;
        }

        public int Execute(IEnumerable<Context> contextsToRun)
        {
            // TODO implement execution cancel

            int count = 0;

            foreach (var context in contextsToRun)
            {
                context.Run(executionReporter, false);

                context.AssignExceptions();

                count += context.AllExamples().Count();
            }

            return count;
        }

        readonly ILiveFormatter executionReporter;
    }
}
