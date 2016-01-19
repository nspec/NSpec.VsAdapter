using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NSpec.VsAdapter.Discovery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSpec.VsAdapter.TestAdapter
{
    public class TestCaseMapper : ITestCaseMapper
    {
        public TestCase FromDiscoveredExample(DiscoveredExample discoveredExample)
        {
            var testCase = new TestCase(discoveredExample.FullName, Constants.ExecutorUri, discoveredExample.SourceAssembly)
                {
                    DisplayName = discoveredExample.FullName,
                    CodeFilePath = discoveredExample.SourceFilePath,
                    LineNumber = discoveredExample.SourceLineNumber,
                };

            var traits = discoveredExample.Tags.Select(tag => new Trait(tag, null));

            testCase.Traits.AddRange(traits);

            return testCase;
        }
    }
}
