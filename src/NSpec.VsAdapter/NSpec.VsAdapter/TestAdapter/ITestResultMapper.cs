using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NSpec.VsAdapter.Execution;

namespace NSpec.VsAdapter.TestAdapter
{
    public interface ITestResultMapper
    {
        TestResult FromExecutedExample(ExecutedExample example, string binaryPath);
    }
}
