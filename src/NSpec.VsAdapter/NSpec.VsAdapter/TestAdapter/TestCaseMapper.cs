using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NSpec.VsAdapter.Core.Discovery;
using System.Linq;

namespace NSpec.VsAdapter.TestAdapter
{
    public class TestCaseMapper : ITestCaseMapper
    {
        public TestCase FromDiscoveredExample(DiscoveredExample discoveredExample)
        {
            var testCase = new TestCase(discoveredExample.FullName, Constants.ExecutorUri, discoveredExample.SourceAssembly)
                {
                    DisplayName = BeautifyForDisplay(discoveredExample.FullName),
                    CodeFilePath = discoveredExample.SourceFilePath,
                    LineNumber = discoveredExample.SourceLineNumber,
                };

            var traits = discoveredExample.Tags
                .Select(tag => tag.Replace('_', ' '))
                .Select(tag => new Trait(tag, null));

            testCase.Traits.AddRange(traits);

            return testCase;
        }

        // beautification idea taken from https://github.com/osoftware/NSpecTestAdapter/blob/master/NSpec.TestAdapter/TestCaseDTO.cs

        string BeautifyForDisplay(string fullName)
        {
            string displayName;

            // chop leading, redundant 'nspec. ' context

            const string nspecPrefix = @"nspec. ";
            const int prefixLength = 7;

            displayName = fullName.StartsWith(nspecPrefix) ? fullName.Substring(prefixLength) : fullName;

            // replace context separator

            const string originalSeparator = @". ";
            const string displaySeparator = @" › ";

            displayName = displayName.Replace(originalSeparator, displaySeparator);

            return displayName;
        }
    }
}
