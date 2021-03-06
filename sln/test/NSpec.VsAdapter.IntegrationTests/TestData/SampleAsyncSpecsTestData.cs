using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System.Collections.Generic;
using System.Linq;

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
                                        DisplayName = "AsyncSpec � method context � async context example.",
                                        CodeFilePath = sourceCodeFilePath,
                                        // some line numbers differ between Debug and Release builds
#if DEBUG
                                        LineNumber = 27,
#elif RELEASE
                                        LineNumber = 28,
#endif
                                    }
                                },
                            }
                        },
                        {
                            "#CLASS_LEVEL_CONTEXT#",
                            new Dictionary<string, TestCase>()
                            {
                                {
                                    "async method example",
                                    new TestCase(
                                        "nspec. AsyncSpec. it async method example.",
                                        Constants.ExecutorUri, specAssemblyPath)
                                    {
                                        DisplayName = "AsyncSpec � it async method example.",
                                        CodeFilePath = sourceCodeFilePath,
#if DEBUG
                                        LineNumber = 18,
#elif RELEASE
                                        LineNumber = 19,
#endif
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

            // add implicit traits corresponding to class names
            // add explicit traits corresponding to tags

            All.Where(tc => tc.FullyQualifiedName.Contains("AsyncSpec"))
                .Do(tc => tc.Traits.Add("AsyncSpec", null));
        }
    }

    /*
    public class SampleAsyncSpecsTestOutputData
    {

    }
     */
}
