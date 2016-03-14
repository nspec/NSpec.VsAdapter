using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using NSpec.VsAdapter.IntegrationTests.TestData;
using NSpec.VsAdapter.Settings;
using NSpec.VsAdapter.TestAdapter;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.IntegrationTests.Discovery
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
            var expected = SampleSpecsTestCaseData.All;

            var actual = sink.TestCases;

            actual.Should().HaveCount(expected.Count());

            actual.ShouldAllBeEquivalentTo(expected);
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
