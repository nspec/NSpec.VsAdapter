using FluentAssertions;
using NSpec.VsAdapter.TestAdapter;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;

namespace NSpec.VsAdapter.IntegrationTests.Execution
{
    [TestFixture]
    [Category("Integration.TestExecution")]
    public class when_executing_tests_base
    {
        protected NSpecTestExecutor executor;

        protected CollectingFrameworkHandle handle;
        protected IRunContext runContext;
        protected readonly string[] sources;

        public when_executing_tests_base()
        {
            sources = new string[] 
            { 
                TestConstants.SampleSpecsDllPath,
                TestConstants.SampleSystemDllPath,
            };
        }

        [SetUp]
        public virtual void before_each()
        {
            runContext = null;

            var consoleLogger = new ConsoleLogger();

            handle = new CollectingFrameworkHandle();

            executor = new NSpecTestExecutor();
        }

        [TearDown]
        public virtual void after_each()
        {
            executor.Dispose();
        }
    }

    public class when_executing_all_tests : when_executing_tests_base
    {
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
        [Ignore("Yet to be implemented")]
        public void it_should_report_result_of_all_examples()
        {
            var expected = SampleSpecsTestCaseData.All.Select(tc => new TestResult(tc)
            {
                Outcome = SampleSpecsTestOutcomeData.ByTestCaseFullName[tc.FullyQualifiedName],
            });

            var actual = handle.Results;

            actual.ShouldAllBeEquivalentTo(expected);
        }
    }
}
