using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NSpec.VsAdapter.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.TestAdapter
{
    public interface ITestResultMapper
    {
        TestResult FromNSpecResult(NSpecResult nspecResult);
    }
}
