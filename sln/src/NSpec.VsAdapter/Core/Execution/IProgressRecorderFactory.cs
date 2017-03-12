using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;

namespace NSpec.VsAdapter.Core.Execution
{
    public interface IProgressRecorderFactory
    {
        IProgressRecorder Create(ITestExecutionRecorder testExecutionRecorder);
    }
}
