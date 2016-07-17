using NSpec.Domain.Formatters;
using NSpec.VsAdapter.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Execution
{
    public class ContextExecutorFactory : IContextExecutorFactory
    {
        public IContextExecutor Create(ILiveFormatter executionReporter, ICrossDomainLogger logger)
        {
            return new ContextExecutor(executionReporter, logger);
        }
    }
}
