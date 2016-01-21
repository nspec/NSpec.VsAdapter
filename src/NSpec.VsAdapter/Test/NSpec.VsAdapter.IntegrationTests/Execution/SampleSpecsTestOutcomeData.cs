using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.IntegrationTests.Execution
{
    public static class SampleSpecsTestOutcomeData
    {
        public readonly static Dictionary<string, TestOutcome> ByTestCaseFullName;

        static SampleSpecsTestOutcomeData()
        {
            ByTestCaseFullName = new Dictionary<string, TestOutcome>()
            {
                {
                    "nspec. ParentSpec. method context 1. parent example 1A.", 
                    TestOutcome.Passed
                },
                {
                    "nspec. ParentSpec. method context 1. parent example 1B.", 
                    TestOutcome.Passed
                },
                {
                    "nspec. ParentSpec. method context 2. parent example 2A.", 
                    TestOutcome.Passed
                },
                {
                    "nspec. ParentSpec. ChildSpec. method context 3. child example 3A skipped.", 
                    TestOutcome.Skipped
                },
                {
                    "nspec. ParentSpec. ChildSpec. method context 4. child example 4A.", 
                    TestOutcome.Passed
                },
            };
        }
    }
}
