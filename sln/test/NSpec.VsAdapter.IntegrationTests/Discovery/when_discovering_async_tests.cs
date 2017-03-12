using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NSpec.VsAdapter.IntegrationTests.TestData;
using System.Collections.Generic;

namespace NSpec.VsAdapter.IntegrationTests.Discovery
{
    public class when_discovering_async_tests : when_discovering_tests_base
    {
        protected override string[] BuildSources()
        {
            return new string[]
            {
                TestConstants.SampleAsyncSpecsDllPath,
                TestConstants.SampleAsyncSystemDllPath,
            };
        }

        protected override IEnumerable<TestCase> BuildExpecteds()
        {
            return SampleAsyncSpecsTestCaseData.All;
        }
    }
}
