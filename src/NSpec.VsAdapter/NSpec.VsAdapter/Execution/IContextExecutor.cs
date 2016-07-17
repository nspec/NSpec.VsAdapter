using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Execution
{
    public interface IContextExecutor
    {
        int Execute(IEnumerable<RunnableContext> runnableContexts);
    }
}
