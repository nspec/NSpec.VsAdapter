using System;

namespace NSpec.VsAdapter.Execution
{
    public interface IProgressRecorder : IDisposable
    {
        string BinaryPath { set; }

        void RecordExecutedExample(ExecutedExample executedExample);
    }
}
