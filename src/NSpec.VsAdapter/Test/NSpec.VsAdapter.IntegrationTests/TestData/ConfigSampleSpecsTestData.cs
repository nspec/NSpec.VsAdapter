using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.IntegrationTests.TestData
{
    public static class ConfigSampleSpecsTestCaseData
    {
        public readonly static IEnumerable<TestCase> All;

        static ConfigSampleSpecsTestCaseData()
        {
            string specAssemblyPath = TestConstants.ConfigSampleSpecsDllPath;
            string sourceCodeFilePath = TestUtils.FirstCharToLower(TestConstants.ConfigSampleSpecsSourcePath);

            All = new TestCase[]
            {
                new TestCase(
                    "nspec. desc_SystemWithSettings. method context. should return app settings value.", 
                    Constants.ExecutorUri, specAssemblyPath)
                {
                    DisplayName = "nspec. desc_SystemWithSettings. method context. should return app settings value.", 
                    CodeFilePath = sourceCodeFilePath,
                    LineNumber = 19,
                }
            };
        }
    }

    public static class ConfigSampleSpecsTestOutcomeData
    {
        public readonly static Dictionary<string, TestOutcome> ByTestCaseFullName;

        static ConfigSampleSpecsTestOutcomeData()
        {
            ByTestCaseFullName = new Dictionary<string, TestOutcome>()
            {
                {
                    "nspec. desc_SystemWithSettings. method context. should return app settings value.", 
                    TestOutcome.Passed
                },
            };
        }
    }
}
