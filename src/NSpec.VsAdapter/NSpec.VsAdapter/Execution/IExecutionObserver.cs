using NSpec.Domain.Formatters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Execution
{
    public interface IExecutionObserver : ILiveFormatter
    {
        string BinaryPath { set; }
    }
}
