using NSpec.Domain.Formatters;
using NSpec.VsAdapter.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Core.Execution.Target
{
    public interface IContextExecutorFactory
    {
        IContextExecutor Create(ILiveFormatter executionReporter, ICrossDomainLogger logger);
    }
}
