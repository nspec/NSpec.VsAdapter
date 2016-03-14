using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                        ErrorMessage = "it[\"child example 5-1A failing\"] = systemUnderTest.IsAlwaysTrue() should be False but was True",
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
