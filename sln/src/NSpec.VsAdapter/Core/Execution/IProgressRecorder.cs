using System;

namespace NSpec.VsAdapter.Core.Execution
{
    public interface IProgressRecorder : IDisposable
    {
        string BinaryPath { set; }

        void RecordExecutedExample(ExecutedExample executedExample);
    }
}
