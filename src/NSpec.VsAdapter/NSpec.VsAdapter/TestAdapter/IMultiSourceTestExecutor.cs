using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;

namespace NSpec.VsAdapter.TestAdapter
{
    public interface IMultiSourceTestExecutor
    {
        void RunTests(IFrameworkHandle frameworkHandle, IRunContext runContext);

        void CancelRun();
    }
}
