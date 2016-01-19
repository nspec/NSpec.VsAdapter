using AutofacContrib.NSubstitute;
using FluentAssertions;
using FluentAssertions.Equivalency;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NSpec.Domain;
using NSpec.VsAdapter.Execution;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.UnitTests.Execution
{
    [TestFixture]
    [Category("TestResultMapper")]
    public class TestResultMapper_desc
    {
        TestResultMapper mapper;

        AutoSubstitute autoSubstitute;

        Context someContext;
        Action someAction;
        Uri someUri;
        const string somePath = @".\path\to\some\dummy-library.dll";
        const string tagText = "some-tag another-tag";

        [SetUp]
        public virtual void before_each()
        {
            autoSubstitute = new AutoSubstitute();

            someContext = new Context("some context");
            someAction = () => { };
            someUri = new Uri("http://www.example.com");

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
            var someExample = new Example("some passed example", tagText, someAction, false)
            {
                Context = someContext,
                HasRun = true,
            };

            var someTestCase = new TestCase(someExample.FullName(), someUri, somePath);

            var expected = new TestResult(someTestCase)
            {
                Outcome = TestOutcome.Passed,
            };

            var actual = mapper.FromExample(someExample);

            actual.ShouldBeEquivalentTo(expected, SetMatchingOptions);
        }

        [Test]
        public void it_should_map_failed_example()
        {
            var someError = new DummyTestException();

            var someExample = new Example("some failed example", tagText, someAction, false)
            {
                Context = someContext,
                HasRun = true,
                Exception = someError,
            };

            var someTestCase = new TestCase(someExample.FullName(), someUri, somePath);

            var expected = new TestResult(someTestCase)
            {
                Outcome = TestOutcome.Failed,
                ErrorMessage = someError.Message,
                ErrorStackTrace = someError.StackTrace,
            };

            var actual = mapper.FromExample(someExample);

            actual.ShouldBeEquivalentTo(expected, SetMatchingOptions);
        }

        [Test]
        public void it_should_map_pending_example()
        {
            var someExample = new Example("some pending example", tagText, someAction, true)
            {
                Context = someContext,
                HasRun = false,
            };

            var someTestCase = new TestCase(someExample.FullName(), someUri, somePath);

            var expected = new TestResult(someTestCase)
            {
                Outcome = TestOutcome.Skipped,
            };

            var actual = mapper.FromExample(someExample);

            actual.ShouldBeEquivalentTo(expected, SetMatchingOptions);
        }

        static EquivalencyAssertionOptions<TestResult> SetMatchingOptions(EquivalencyAssertionOptions<TestResult> opts)
        {
            return opts
                .Including(tr => tr.Outcome)
                .Including(tr => tr.ErrorMessage)
                .Including(tr => tr.ErrorStackTrace);
        }
    }
}
