using FluentAssertions.Equivalency;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using NSpec.VsAdapter.TestAdapter;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.IntegrationTests.Execution
{
    [TestFixture]
    [Category("Integration.TestExecution")]
    public class when_executing_tests_base
    {
        protected NSpecTestExecutor executor;

        protected CollectingFrameworkHandle handle;
        protected IRunContext runContext;

        [SetUp]
        public virtual void before_each()
        {
            runContext = new EmptyRunContext();

            var consoleLogger = new ConsoleLogger();

            handle = new CollectingFrameworkHandle(consoleLogger);

            executor = new NSpecTestExecutor();
        }

        [TearDown]
        public virtual void after_each()
        {
            executor.Dispose();
        }

        protected static TestResult MapTestCaseToResult(Dictionary<string, TestOutcome> outcomeByFullNameMap, TestCase testCase)
        {
            var testResult = new TestResult(testCase)
            {
                Outcome = outcomeByFullNameMap[testCase.FullyQualifiedName],
            };

            return testResult;
        }

        protected static EquivalencyAssertionOptions<TestResult> TestResultMatchingOptions(EquivalencyAssertionOptions<TestResult> opts)
        {
            return opts
                .Including(tr => tr.Outcome)
                .Including(tr => tr.ErrorMessage)
                .Including(tr => tr.ErrorStackTrace)
                .Including(tr => tr.TestCase.FullyQualifiedName)
                .Including(tr => tr.TestCase.ExecutorUri)
                .Including(tr => tr.TestCase.Source);
        }
    }
}
