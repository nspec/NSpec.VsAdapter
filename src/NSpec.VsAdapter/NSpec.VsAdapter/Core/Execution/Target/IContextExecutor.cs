using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Core.Execution.Target
{
    public interface IContextExecutor
    {
        int Execute(IEnumerable<RunnableContext> runnableContexts);
    }
}
