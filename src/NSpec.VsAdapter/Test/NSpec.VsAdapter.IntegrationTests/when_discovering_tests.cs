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
        string targetDllPath;

        [SetUp]
        public virtual void setup()
        {
            targetDllPath = TestConstants.SampleSpecsDllPath;
            
            var sources = new string[] 
            { 
                targetDllPath,
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
            sink.TestCases.Should().HaveCount(5);
        }

        [Test]
        public void it_should_set_full_names()
        {
            var expected = SampleSpecsTestCaseData.All.Select(tc => tc.FullyQualifiedName);

            var actual = sink.TestCases.Select(tc => tc.FullyQualifiedName);

            actual.ShouldAllBeEquivalentTo(expected);
        }

        [Test]
        public void it_should_set_display_names()
        {
            var expected = SampleSpecsTestCaseData.All.Select(tc => tc.DisplayName);

            var actual = sink.TestCases.Select(tc => tc.DisplayName);

            actual.ShouldAllBeEquivalentTo(expected);
        }

        [Test]
        public void it_should_set_executor_uris()
        {
            var expected = SampleSpecsTestCaseData.All.Select(tc => tc.ExecutorUri).Distinct().Single();

            sink.TestCases.ForEach(tc =>
                {
                    tc.ExecutorUri.Should().Be(expected, "FullName: {0}", tc.FullyQualifiedName);
                });
        }

        [Test]
        public void it_should_set_sources()
        {
            var expected = SampleSpecsTestCaseData.All.Select(tc => tc.Source).Distinct().Single();

            sink.TestCases.ForEach(tc =>
            {
                TestUtils.FirstCharToUpper(tc.Source).Should().Be(expected, "FullName: {0}", tc.FullyQualifiedName);
            });
        }

        [Test]
        public void it_should_set_code_file_paths()
        {
            sink.TestCases.ForEach(tc =>
            {
                string actualFullName = tc.FullyQualifiedName;

                string expected = SampleSpecsTestCaseData.ByTestCaseFullName[actualFullName].CodeFilePath;

                TestUtils.FirstCharToUpper(tc.CodeFilePath).Should().Be(expected, "FullName: {0}", actualFullName);
            });
        }

        [Test]
        public void it_should_set_code_line_numbers()
        {
            sink.TestCases.ForEach(tc =>
            {
                string actualFullName = tc.FullyQualifiedName;

                int expected = SampleSpecsTestCaseData.ByTestCaseFullName[actualFullName].LineNumber;

                tc.LineNumber.Should().Be(expected, "FullName: {0}", actualFullName);
            });
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

            public List<TestCase> TestCases { get; set; }

            public void SendTestCase(TestCase discoveredTestCase)
            {
                TestCases.Add(discoveredTestCase);
            }
        }
    }
}
