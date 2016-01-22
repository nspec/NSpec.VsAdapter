using AutofacContrib.NSubstitute;
using FluentAssertions;
using FluentAssertions.Equivalency;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NSpec.VsAdapter.Execution;
using NSpec.VsAdapter.TestAdapter;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.UnitTests.TestAdapter
{
    [TestFixture]
    [Category("TestResultMapper")]
    public class TestResultMapper_desc
    {
        TestResultMapper mapper;

        AutoSubstitute autoSubstitute;

        readonly Uri someUri = new Uri("http://www.example.com");
        const string somePath = @".\path\to\some\dummy-library.dll";

        [SetUp]
        public virtual void before_each()
        {
            autoSubstitute = new AutoSubstitute();

            mapper = autoSubstitute.Resolve<TestResultMapper>();
        }

        [TearDown]
        public virtual void after_each()
        {
            autoSubstitute.Dispose();
        }

        [Test]
        public void it_should_map_passed_example()
        {
            var someExample = new ExecutedExample()
            {
                FullName = "nspec. some context. some passing example.",
                Pending = false,
                Failed = false,
            };

            var someTestCase = BuildTestCase(someExample);

            var expected = new TestResult(someTestCase)
            {
                Outcome = TestOutcome.Passed,
            };

            var actual = mapper.FromExecutedExample(someExample, somePath);

            actual.ShouldBeEquivalentTo(expected, SetMatchingOptions);
        }

        [Test]
        public void it_should_map_failed_example()
        {
            var someError = new DummyTestException();

            var someExample = new ExecutedExample()
            {
                FullName = "nspec. some context. some failing example.",
                Pending = false,
                Failed = true,
                Exception = someError,
            };

            var someTestCase = BuildTestCase(someExample);

            var expected = new TestResult(someTestCase)
            {
                Outcome = TestOutcome.Failed,
                ErrorMessage = someError.Message,
                ErrorStackTrace = someError.StackTrace,
            };

            var actual = mapper.FromExecutedExample(someExample, somePath);

            actual.ShouldBeEquivalentTo(expected, SetMatchingOptions);
        }

        [Test]
        public void it_should_map_pending_example()
        {
            var someExample = new ExecutedExample()
            {
                FullName = "nspec. some context. some pending example.",
                Pending = true,
                Failed = false,
            };

            var someTestCase = BuildTestCase(someExample);

            var expected = new TestResult(someTestCase)
            {
                Outcome = TestOutcome.Skipped,
            };

            var actual = mapper.FromExecutedExample(someExample, somePath);

            actual.ShouldBeEquivalentTo(expected, SetMatchingOptions);
        }

        TestCase BuildTestCase(ExecutedExample someExample)
        {
            return new TestCase(someExample.FullName, someUri, somePath);
        }

        static EquivalencyAssertionOptions<TestResult> SetMatchingOptions(EquivalencyAssertionOptions<TestResult> opts)
        {
            return opts
                .Including(tr => tr.Outcome)
                .Including(tr => tr.ErrorMessage)
                .Including(tr => tr.ErrorStackTrace)
                .Including(tr => tr.TestCase.Source);
        }
    }
}
