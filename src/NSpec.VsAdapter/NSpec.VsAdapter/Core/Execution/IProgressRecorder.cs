using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Core.Execution
{
    public interface IProgressRecorder : IDisposable
    {
        string BinaryPath { set; }

        void RecordExecutedExample(ExecutedExample executedExample);
    }
}
