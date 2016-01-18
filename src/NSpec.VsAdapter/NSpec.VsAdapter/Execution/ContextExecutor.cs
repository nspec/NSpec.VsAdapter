using NSpec.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSpec.VsAdapter.Execution
{
    public class ContextExecutor
    {
        public ContextExecutor(IExecutionObserver executionObserver)
        {
            this.executionObserver = executionObserver;
        }

        public int Execute(IEnumerable<Context> contextsToRun)
        {
            int count = 0;

            foreach (var context in contextsToRun)
            {
                context.Run(executionObserver, false);

                context.AssignExceptions();

                count += context.AllExamples().Count();
            }

            return count;
        }

        readonly IExecutionObserver executionObserver;
    }
}
