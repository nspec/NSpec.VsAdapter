using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;

namespace NSpec.VsAdapter.Execution
{
    public interface IProgressRecorderFactory
    {
        IProgressRecorder Create(ITestExecutionRecorder testExecutionRecorder);
    }
}
