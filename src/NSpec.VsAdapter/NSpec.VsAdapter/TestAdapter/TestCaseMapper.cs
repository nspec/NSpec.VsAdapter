using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSpec.VsAdapter.TestAdapter
{
    public class TestCaseMapper : ITestCaseMapper
    {
        public TestCase FromSpecification(NSpecSpecification spec)
        {
            return new TestCase(spec.FullName, Constants.ExecutorUri, spec.SourceFilePath);
        }
    }
}
