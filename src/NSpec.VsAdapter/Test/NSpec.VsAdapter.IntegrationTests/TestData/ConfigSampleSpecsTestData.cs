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
                    "nspec. desc SystemWithSettings. method context. should return app settings value.", 
                    Constants.ExecutorUri, specAssemblyPath)
                {
                    DisplayName = "nspec. desc SystemWithSettings. method context. should return app settings value.", 
                    CodeFilePath = sourceCodeFilePath,
                    LineNumber = 19,
                }
            };
        }
    }

    public static class ConfigSampleSpecsTestOutputData
    {
        public readonly static Dictionary<string, TestOutput> ByTestCaseFullName;

        static ConfigSampleSpecsTestOutputData()
        {
            string specAssemblyPath = TestConstants.ConfigSampleSpecsDllPath;

            ByTestCaseFullName = new Dictionary<string, TestOutput>()
            {
                {
                    "nspec. desc SystemWithSettings. method context. should return app settings value.", 
                    new TestOutput()
                    {
                        FullyQualifiedName = "nspec. desc SystemWithSettings. method context. should return app settings value.", 
                        Outcome = TestOutcome.Passed,
                    }
                },
            };
        }
    }
}
