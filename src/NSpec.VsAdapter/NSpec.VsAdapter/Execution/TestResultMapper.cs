using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NSpec.Domain;
using NSpec.VsAdapter.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Execution
{
    public class TestResultMapper : ITestResultMapper
    {
        public TestResult FromExample(ExampleBase example)
        {
            throw new NotImplementedException();
        }
    }
}
