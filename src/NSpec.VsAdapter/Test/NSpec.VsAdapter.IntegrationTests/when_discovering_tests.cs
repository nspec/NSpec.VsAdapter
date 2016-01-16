using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using NSpec.VsAdapter.TestAdapter;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.IntegrationTests
{
    [TestFixture]
    [Category("Integration.TestDiscovery")]
    public class when_discovering_tests
    {
        NSpecTestDiscoverer discoverer;

        CollectingSink sink;

        [SetUp]
        public virtual void before_each()
        {
            var sources = new string[] 
            { 
                TestConstants.SampleSpecsDllPath,
                TestConstants.SampleSystemDllPath,
            };

            IDiscoveryContext discoveryContext = null;

            var consoleLogger = new ConsoleLogger();

            sink = new CollectingSink();

            discoverer = new NSpecTestDiscoverer();

            discoverer.DiscoverTests(sources, discoveryContext, consoleLogger, sink);
        }

        [TearDown]
        public virtual void after_each()
        {
            discoverer.Dispose();
        }

        [Test]
        public void it_should_find_all_examples()
        {
            var expected = SampleSpecsTestCaseData.All;

            var actual = sink.TestCases;

            actual.Should().HaveCount(expected.Count());

            actual.ShouldAllBeEquivalentTo(expected, options => options
                .Excluding(tc => tc.Properties)
                .Excluding(tc => tc.Traits));
        }

        [Test]
        [Ignore("Tags/traits yet to be implemented")]
        public void it_should_detect_tags()
        {
            var tags = sink.TestCases.SelectMany(tc => tc.Traits);

            tags.Where(t => t.Name == "describe DeepThought").Should().HaveCount(4);

            tags.Any(t => t.Name == "describe Earth").Should().BeTrue();
            tags.Any(t => t.Name == "One-should-fail").Should().BeTrue();
            tags.Any(t => t.Name == "One-should-pass").Should().BeTrue();
            tags.Any(t => t.Name == "Should be skipped").Should().BeTrue();
            tags.Any(t => t.Name == "Derived").Should().BeTrue();
        }

        class CollectingSink : ITestCaseDiscoverySink
        {
            public CollectingSink()
            {
                TestCases = new List<TestCase>();
            }

            public List<TestCase> TestCases { get; private set; }

            public void SendTestCase(TestCase discoveredTestCase)
            {
                TestCases.Add(discoveredTestCase);
            }
        }
    }
}
