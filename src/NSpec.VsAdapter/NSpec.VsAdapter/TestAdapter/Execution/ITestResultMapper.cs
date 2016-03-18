using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NSpec.Domain;
using NSpec.VsAdapter.Core.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.TestAdapter.Execution
{
    public interface ITestResultMapper
    {
        TestResult FromExecutedExample(ExecutedExample example, string binaryPath);
    }
}
