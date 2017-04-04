using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace NSpec.VsAdapter.IntegrationTests.TestData
{
    public class TestOutput
    {
        public string FullyQualifiedName { get; set; }

        public TestOutcome Outcome { get; set; }

        public string ErrorMessage { get; set; }
    }
}
