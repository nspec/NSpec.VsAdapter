using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.IntegrationTests.Execution
{
    public class when_executing_selected_tests : when_executing_tests_base
    {
        readonly TestCase[] selectedTestCases;
        readonly TestCase[] runningTestCases;

        public when_executing_selected_tests()
        {
            selectedTestCases = new TestCase[]
            {
                // this is example sits in a context with another example, that should be executed as well
                SampleSpecsTestCaseData.ByTestCaseFullName["nspec. ParentSpec. method context 1. parent example 1B."],

                SampleSpecsTestCaseData.ByTestCaseFullName["nspec. ParentSpec. method context 2. parent example 2A."],

                SampleSpecsTestCaseData.ByTestCaseFullName["nspec. ParentSpec. ChildSpec. method context 3. child example 3A skipped."],
            };

            runningTestCases = new TestCase[]
            {
                SampleSpecsTestCaseData.ByTestCaseFullName["nspec. ParentSpec. method context 1. parent example 1A."],
                SampleSpecsTestCaseData.ByTestCaseFullName["nspec. ParentSpec. method context 1. parent example 1B."],

                SampleSpecsTestCaseData.ByTestCaseFullName["nspec. ParentSpec. method context 2. parent example 2A."],

                SampleSpecsTestCaseData.ByTestCaseFullName["nspec. ParentSpec. ChildSpec. method context 3. child example 3A skipped."],
            };
        }

        public override void before_each()
        {
            base.before_each();

            executor.RunTests(selectedTestCases, runContext, handle);
        }

        [Test]
        [Ignore("Yet to be implemented")]
        public void it_should_start_selected_examples()
        {
            throw new NotImplementedException();
        }

        [Test]
        [Ignore("Yet to be implemented")]
        public void it_should_end_selected_examples()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void it_should_report_result_of_selected_examples()
        {
            var selectedFullNames = runningTestCases.Select(tc => tc.FullyQualifiedName);
            
            Func<TestCase, TestResult> mapToTestResult =
                tc => MapTestCaseToResult(SampleSpecsTestOutcomeData.ByTestCaseFullName, tc);

            IEnumerable<TestResult> expected = SampleSpecsTestCaseData
                .ByTestCaseFullName.Where(pair =>
                {
                    string fullName = pair.Key;

                    return selectedFullNames.Contains(fullName);
                })
                .Select(pair => mapToTestResult(pair.Value));

            var actual = handle.Results;

            actual.ShouldAllBeEquivalentTo(expected, TestResultMatchingOptions);
        }
    }
}
