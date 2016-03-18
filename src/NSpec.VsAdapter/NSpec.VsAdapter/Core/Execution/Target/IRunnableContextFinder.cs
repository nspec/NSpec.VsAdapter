using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Core.Execution.Target
{
    public interface IRunnableContextFinder
    {
        IEnumerable<RunnableContext> Find(string binaryPath, string[] exampleFullNames);
    }
}
