using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NSpec.VsAdapter.TestAdapter.Discovery;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

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

            actuals.Should().HaveCount(expecteds.Count());

            actuals.ShouldAllBeEquivalentTo(expecteds);
        }

        protected abstract string[] BuildSources();

        protected abstract IEnumerable<TestCase> BuildExpecteds();
    }
}
