using FluentAssertions;
using FluentAssertions.Equivalency;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NSpec.VsAdapter.TestAdapter.Discovery;
using NUnit.Framework;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

namespace NSpec.VsAdapter.IntegrationTests.Discovery
{
    [TestFixture]
    [Category("Integration.TestDiscovery")]
    public abstract class when_discovering_tests_base
    {
        NSpecTestDiscoverer discoverer;

        CollectingSink sink;

        [SetUp]
        public virtual void before_each()
        {
            var sources = BuildSources();

            var discoveryContext = new EmptyDiscoveryContext();

            var consoleLogger = new ConsoleLogger();

            sink = new CollectingSink();

            discoverer = new NSpecTestDiscoverer();

            discoverer.DiscoverTests(sources, discoveryContext, consoleLogger, sink);
        }

        [TearDown]
        public virtual void after_each()
        {
            if (discoverer != null) discoverer.Dispose();
        }

        [Test]
        public void it_should_find_all_examples_with_their_data()
        {
            var expecteds = BuildExpecteds();

            var actuals = sink.TestCases;

            actuals.ShouldAllBeEquivalentTo(expecteds, ConfigureTestCaseMatching, this.GetType().Name);
        }

        protected abstract string[] BuildSources();

        protected abstract IEnumerable<TestCase> BuildExpecteds();

        protected static EquivalencyAssertionOptions<TestCase> ConfigureTestCaseMatching(EquivalencyAssertionOptions<TestCase> opts)
        {
            return opts.Using(new CodeFilePathEquivalency());
        }

        protected class CodeFilePathEquivalency : IEquivalencyStep
        {
            public bool CanHandle(IEquivalencyValidationContext context, IEquivalencyAssertionOptions config)
            {
                return (context.SelectedMemberPath == "CodeFilePath");
            }

            public bool Handle(IEquivalencyValidationContext context, IEquivalencyValidator parent, IEquivalencyAssertionOptions config)
            {
                string actualCodeFilePath = (string)context.Subject;
                string expectedCodeFilePath = (string)context.Expectation;

                // avoid mismatches due to drive letter casing
                if (leadingDriveRegex.IsMatch(expectedCodeFilePath))
                {
                    actualCodeFilePath = TestUtils.FirstCharToUpper(actualCodeFilePath);
                    expectedCodeFilePath = TestUtils.FirstCharToUpper(expectedCodeFilePath);
                }

                return (actualCodeFilePath == expectedCodeFilePath);
            }

            readonly Regex leadingDriveRegex = new Regex(@"^[a-zA-Z]:\\");
        }
    }
}
