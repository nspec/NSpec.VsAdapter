using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System.Collections.Generic;

namespace NSpec.VsAdapter.IntegrationTests.TestData
{
    public static class SampleSpecsTestOutputData
    {
        public readonly static Dictionary<string, TestOutput> ByTestCaseFullName;

        static SampleSpecsTestOutputData()
        {
            string specAssemblyPath = TestConstants.SampleSpecsDllPath;

            ByTestCaseFullName = new Dictionary<string, TestOutput>()
            {
                {
                    "nspec. ParentSpec. method context 1. parent example 1A.",
                    new TestOutput()
                    {
                        FullyQualifiedName = "nspec. ParentSpec. method context 1. parent example 1A.",
                        Outcome = TestOutcome.Passed,
                    }
                },
                {
                    "nspec. ParentSpec. method context 1. parent example 1B.",
                    new TestOutput()
                    {
                        FullyQualifiedName = "nspec. ParentSpec. method context 1. parent example 1B.",
                        Outcome = TestOutcome.Passed,
                    }
                },
                {
                    "nspec. ParentSpec. method context 2. parent example 2A.",
                    new TestOutput()
                    {
                        FullyQualifiedName = "nspec. ParentSpec. method context 2. parent example 2A.",
                        Outcome = TestOutcome.Passed,
                    }
                },
                {
                    "nspec. ParentSpec. ChildSpec. method context 3. child example 3A skipped.",
                    new TestOutput()
                    {
                        FullyQualifiedName = "nspec. ParentSpec. ChildSpec. method context 3. child example 3A skipped.",
                        Outcome = TestOutcome.Skipped,
                    }
                },
                {
                    "nspec. ParentSpec. ChildSpec. method context 4. child example 4A.",
                    new TestOutput()
                    {
                        FullyQualifiedName = "nspec. ParentSpec. ChildSpec. method context 4. child example 4A.",
                        Outcome = TestOutcome.Passed,
                    }
                },
                {
                    "nspec. ParentSpec. ChildSpec. method context 5. sub context 5-1. child example 5-1A failing.",
                    new TestOutput()
                    {
                        FullyQualifiedName = "nspec. ParentSpec. ChildSpec. method context 5. sub context 5-1. child example 5-1A failing.",
                        Outcome = TestOutcome.Failed,
                        // Shouldy error messages change in Release builds
#if DEBUG
                        ErrorMessage = "it[\"child example 5-1A failing\"] = systemUnderTest.IsAlwaysTrue()\n    should be\nFalse\n    but was\nTrue",
#elif RELEASE
                        ErrorMessage = "Shouldly uses your source code to generate its great error messages, build your test project with full debug information to get better error messages\nThe provided expression\n    should be\nFalse\n    but was\nTrue",
#endif
                    }
                },
                {
                    "nspec. ParentSpec. ChildSpec. method context 5. sub context 5-1. child example 5-1B.",
                    new TestOutput()
                    {
                        FullyQualifiedName = "nspec. ParentSpec. ChildSpec. method context 5. sub context 5-1. child example 5-1B.",
                        Outcome = TestOutcome.Passed,
                    }
                },
            };
        }
    }
}
