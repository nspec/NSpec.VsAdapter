using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Execution
{
    public interface IExecutionCanceler
    {
        bool IsCanceled { get; }
    }
}
