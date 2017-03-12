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
    public class when_target_has_app_config : when_executing_tests_base
    {
        readonly string[] sources = new string[] 
        { 
            TestConstants.ConfigSampleSpecsDllPath,
        };

        public override void before_each()
        {
            base.before_each();

            executor.RunTests(sources, runContext, handle);
        }

        [Test]
        public void it_should_access_app_settings()
        {
            Func<TestCase, TestResult> mapToTestResult =
                tc => MapTestCaseToResult(ConfigSampleSpecsTestOutputData.ByTestCaseFullName, tc);

            var expected = ConfigSampleSpecsTestCaseData.All.Select(mapToTestResult);

            var actual = handle.Results;

            actual.ShouldAllBeEquivalentTo(expected, TestResultMatchingOptions);
        }
    }
}
