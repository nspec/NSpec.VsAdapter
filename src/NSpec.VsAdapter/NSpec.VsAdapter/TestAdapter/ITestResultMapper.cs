using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NSpec.VsAdapter.Core.Execution;

namespace NSpec.VsAdapter.TestAdapter
{
    public interface ITestResultMapper
    {
        TestResult FromExecutedExample(ExecutedExample example, string binaryPath);
    }
}
