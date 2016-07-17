using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System.Collections.Generic;

namespace NSpec.VsAdapter.TestAdapter
{
    public interface IMultiSourceTestExecutorFactory
    {
        IMultiSourceTestExecutor Create(IEnumerable<string> sources);

        IMultiSourceTestExecutor Create(IEnumerable<TestCase> testCases);
    }
}
