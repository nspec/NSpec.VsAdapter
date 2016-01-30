using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NSpec.VsAdapter.IntegrationTests.TestData;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.IntegrationTests.Execution
{
    public class when_executing_all_tests : when_executing_tests_base
    {
        readonly string[] sources = new string[] 
        { 
            TestConstants.SampleSpecsDllPath,
            TestConstants.SampleSystemDllPath,
        };

        public override void before_each()
        {
            base.before_each();

            executor.RunTests(sources, runContext, handle);
        }

        [Test]
        [Ignore("Yet to be implemented")]
        public void it_should_start_all_examples()
        {
            var expected = SampleSpecsTestCaseData.All;

            var actual = handle.StartedTestCases;

            actual.ShouldAllBeEquivalentTo(expected);
        }

        [Test]
        [Ignore("Yet to be implemented")]
        public void it_should_end_all_examples()
        {
            var expected = SampleSpecsTestOutcomeData.ByTestCaseFullName;

            var actual = handle.EndedTestInfo
                .ToDictionary(info => info.Item1.FullyQualifiedName, info => info.Item2);

            actual.ShouldAllBeEquivalentTo(expected);
        }

        [Test]
        public void it_should_report_result_of_all_examples()
        {
            Func<TestCase, TestResult> mapToTestResult = 
                tc => MapTestCaseToResult(SampleSpecsTestOutcomeData.ByTestCaseFullName, tc);

            var expected = SampleSpecsTestCaseData.All.Select(mapToTestResult);

            var actual = handle.Results;

            actual.ShouldAllBeEquivalentTo(expected, TestResultMatchingOptions);
        }
    }
}
