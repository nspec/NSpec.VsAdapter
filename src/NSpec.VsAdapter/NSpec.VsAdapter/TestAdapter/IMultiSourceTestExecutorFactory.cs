using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.TestAdapter
{
    public interface IMultiSourceTestExecutorFactory
    {
        IMultiSourceTestExecutor Create(IEnumerable<string> sources);

        IMultiSourceTestExecutor Create(IEnumerable<TestCase> testCases);
    }
}
