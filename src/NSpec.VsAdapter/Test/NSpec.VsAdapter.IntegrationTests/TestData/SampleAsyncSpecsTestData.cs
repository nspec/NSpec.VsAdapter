using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.IntegrationTests.TestData
{
    public class SampleAsyncSpecsTestCaseData
    {
        public readonly static
            Dictionary<string, Dictionary<string, Dictionary<string, TestCase>>> ByClassMethodExampleName;

        public readonly static Dictionary<string, TestCase> ByTestCaseFullName;

        public readonly static IEnumerable<TestCase> All;

        static SampleAsyncSpecsTestCaseData()
        {
            string specAssemblyPath = TestConstants.SampleAsyncSpecsDllPath;
            string sourceCodeFilePath = TestUtils.FirstCharToLower(TestConstants.SampleAsyncSpecsSourcePath);

            // TODO add method-level example too

            ByClassMethodExampleName = new Dictionary<string, Dictionary<string, Dictionary<string, TestCase>>>()
            {
                {
                    "SampleAsyncSpecs.AsyncSpec",
                    new Dictionary<string, Dictionary<string, TestCase>>()
                    {
                        {
                            "method_context",
                            new Dictionary<string, TestCase>()
                            {
                                {
                                    "async context example",
                                    new TestCase(
                                        "nspec. AsyncSpec. method context. async context example.",
                                        Constants.ExecutorUri, specAssemblyPath)
                                    {
                                        DisplayName = "AsyncSpec › method context › async context example.",
                                        CodeFilePath = sourceCodeFilePath,
                                        LineNumber = 26,
                                    }
                                },
                            }
                        },
                    }
                },
            };

            All = ByClassMethodExampleName
                .SelectMany(byClassName => byClassName.Value)
                .SelectMany(byMethodName => byMethodName.Value)
                .Select(byExampleName => byExampleName.Value);

            ByTestCaseFullName = All.ToDictionary(tc => tc.FullyQualifiedName, tc => tc);
        }
    }

    /*
    public class SampleAsyncSpecsTestOutputData
    {

    }
     */
}
