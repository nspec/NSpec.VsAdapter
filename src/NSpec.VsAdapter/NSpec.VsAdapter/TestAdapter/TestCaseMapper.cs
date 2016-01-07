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
            var testCase = new TestCase(spec.FullName, Constants.ExecutorUri, spec.SourceAssembly)
                {
                    DisplayName = spec.FullName,
                    CodeFilePath = spec.SourceFilePath,
                    LineNumber = spec.SourceLineNumber,
                };

            var traits = spec.Tags.Select(tag => new Trait(tag, null));

            testCase.Traits.AddRange(traits);

            return testCase;
        }
    }
}
