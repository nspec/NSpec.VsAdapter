using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.IntegrationTests
{
    public static class SampleSpecsTestCaseData
    {
        public readonly static 
            Dictionary<string, Dictionary<string, Dictionary<string, TestCase>>> ByClassMethodExampleName;

        public readonly static Dictionary<string, TestCase> ByTestCaseFullName;

        public readonly static IEnumerable<TestCase> All;

        static SampleSpecsTestCaseData()
        {
            string specAssemblyPath = TestConstants.SampleSpecsDllPath;
            string sourceCodeFilePath = TestUtils.FirstCharToLower(TestConstants.SampleSpecsSourcePath);

            ByClassMethodExampleName = new Dictionary<string, Dictionary<string, Dictionary<string, TestCase>>>()
            {
                { 
                    "SampleSpecs.ParentSpec", 
                    new Dictionary<string, Dictionary<string, TestCase>>()
                    {
                        {
                            "method_context_1",
                            new Dictionary<string, TestCase>()
                            {
                                {
                                    "parent example 1A",
                                    new TestCase(
                                        "nspec. ParentSpec. method context 1. parent example 1A.", 
                                        Constants.ExecutorUri, specAssemblyPath)
                                    {
                                        DisplayName = "nspec. ParentSpec. method context 1. parent example 1A.",
                                        CodeFilePath = sourceCodeFilePath,
                                        LineNumber = 19,
                                    }
                                },
                                {
                                    "parent example 1B",
                                    new TestCase(
                                        "nspec. ParentSpec. method context 1. parent example 1B.", 
                                        Constants.ExecutorUri, specAssemblyPath)
                                    {
                                        DisplayName = "nspec. ParentSpec. method context 1. parent example 1B.",
                                        CodeFilePath = sourceCodeFilePath,
                                        LineNumber = 21,
                                    }
                                },
                            }
                        },
                        {
                            "method_context_2",
                            new Dictionary<string, TestCase>()
                            {
                                {
                                    "parent example 2A",
                                    new TestCase(
                                        "nspec. ParentSpec. method context 2. parent example 2A.", 
                                        Constants.ExecutorUri, specAssemblyPath)
                                    {
                                        DisplayName = "nspec. ParentSpec. method context 2. parent example 2A.",
                                        CodeFilePath = sourceCodeFilePath,
                                        LineNumber = 26,
                                    }
                                },
                            }
                        },
                    }
                },
                { 
                    "SampleSpecs.ChildSpec", 
                    new Dictionary<string, Dictionary<string, TestCase>>()
                    {
                        {
                            "method_context_3",
                            new Dictionary<string, TestCase>()
                            {
                                {
                                    "child example 3A skipped",
                                    new TestCase(
                                        "nspec. ParentSpec. ChildSpec. method context 3. child example 3A skipped.", 
                                        Constants.ExecutorUri, specAssemblyPath)
                                    {
                                        DisplayName = "nspec. ParentSpec. ChildSpec. method context 3. child example 3A skipped.",
                                        // no source code info available for pending tests
                                        CodeFilePath = String.Empty,
                                        LineNumber = 0,
                                    }
                                },
                            }
                        },
                        {
                            "method_context_4",
                            new Dictionary<string, TestCase>()
                            {
                                {
                                    "child example 4A",
                                    new TestCase(
                                        "nspec. ParentSpec. ChildSpec. method context 4. child example 4A.", 
                                        Constants.ExecutorUri, specAssemblyPath)
                                    {
                                        DisplayName = "nspec. ParentSpec. ChildSpec. method context 4. child example 4A.",
                                        CodeFilePath = sourceCodeFilePath,
                                        LineNumber = 41,
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

            // add traits corresponding to tags

            All.Where(tc => tc.FullyQualifiedName.Contains("ParentSpec"))
                .Do(tc => tc.Traits.Add("ParentSpec", null));

            All.Where(tc => tc.FullyQualifiedName.Contains("method context 1"))
                .Do(tc => 
                {
                    tc.Traits.Add("Tag-1A", null);
                    tc.Traits.Add("Tag-1B", null);
                });

            All.Where(tc => tc.FullyQualifiedName.Contains("ChildSpec"))
                .Do(tc =>
                {
                    tc.Traits.Add("ChildSpec", null);
                    tc.Traits.Add("Tag-Child", null);
                });

            All.Where(tc => tc.FullyQualifiedName.Contains("method context 3"))
                .Do(tc => tc.Traits.Add("Tag-Child-example-skipped", null));
        }

        // adapted from https://github.com/mattflo/NSpec/blob/master/NSpec/Extensions.cs

        public static IEnumerable<TestCase> Do(this IEnumerable<TestCase> source, Action<TestCase> action)
        {
            foreach (var t in source)
                action(t);

            return source;
        }
    }
}
