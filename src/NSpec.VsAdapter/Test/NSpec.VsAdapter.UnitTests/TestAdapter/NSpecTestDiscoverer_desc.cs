using AutofacContrib.NSubstitute;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using NSpec.VsAdapter.Discovery;
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
    [Category("NSpecTestDiscoverer")]
    public abstract class NSpecTestDiscoverer_desc_base
    {
        protected NSpecTestDiscoverer discoverer;

        protected AutoSubstitute autoSubstitute;
        protected List<TestCase> testCases;
        protected ITestCaseDiscoverySink discoverySink;
        protected IBinaryTestDiscoverer binaryTestDiscoverer;

        [SetUp]
        public virtual void before_each()
        {
            autoSubstitute = new AutoSubstitute();

            testCases = new List<TestCase>();
            discoverySink = autoSubstitute.Resolve<ITestCaseDiscoverySink>();
            discoverySink.When(sink => sink.SendTestCase(Arg.Any<TestCase>())).Do(callInfo =>
                {
                    var discoveredTestCase = callInfo.Arg<TestCase>();

                    testCases.Add(discoveredTestCase);
                });

            binaryTestDiscoverer = autoSubstitute.Resolve<IBinaryTestDiscoverer>();

            discoverer = autoSubstitute.Resolve<NSpecTestDiscoverer>();
        }

        [TearDown]
        public virtual void after_each()
        {
            autoSubstitute.Dispose();
            discoverer.Dispose();
        }
    }

    public class NSpecTestDiscoverer_when_discovering : NSpecTestDiscoverer_desc_base
    {
        OutputLogger outputLogger;
        string[] sources;
        Dictionary<string, NSpecSpecification[]> specificationBySource;

        public NSpecTestDiscoverer_when_discovering()
        {
            string source1 = @".\path\to\some\dummy-library.dll";
            string source2 = @".\other\path\to\another-dummy-library.dll";

            sources = new string[]
            {
                source1,
                source2,
            };

            specificationBySource = new Dictionary<string, NSpecSpecification[]>()
            {
                { 
                    source1, 
                    new NSpecSpecification[] 
                    { 
                        new NSpecSpecification() { SourceFilePath = source1, FullName = "source-1-spec-A", },
                        new NSpecSpecification() { SourceFilePath = source1, FullName = "source-1-spec-B", },
                        new NSpecSpecification() { SourceFilePath = source1, FullName = "source-1-spec-C", },
                    }
                },
                { 
                    source2, 
                    new NSpecSpecification[] 
                    { 
                        new NSpecSpecification() { SourceFilePath = source2, FullName = "source-2-spec-A", },
                        new NSpecSpecification() { SourceFilePath = source2, FullName = "source-2-spec-B", },
                    }
                },
            };
        }

        public override void before_each()
        {
            base.before_each();

            binaryTestDiscoverer.Discover(null, null, null).ReturnsForAnyArgs(callInfo =>
                {
                    string assemblyPath = callInfo.Arg<string>();
                    
                    if (sources.Contains(assemblyPath))
                    {
                        return specificationBySource[assemblyPath];
                    }
                    else
                    {
                        return new NSpecSpecification[0];
                    }
                });

            var testCaseMapper = autoSubstitute.Resolve<ITestCaseMapper>();
            testCaseMapper.FromSpecification(null).ReturnsForAnyArgs(callInfo =>
                {
                    var spec = callInfo.Arg<NSpecSpecification>();

                    var testCase = new TestCase(spec.FullName, Constants.ExecutorUri, spec.SourceFilePath);

                    return testCase;
                });

            var messageLogger = autoSubstitute.Resolve<IMessageLogger>();

            outputLogger = autoSubstitute.Resolve<OutputLogger>();
            var loggerFactory = autoSubstitute.Resolve<ILoggerFactory>();
            loggerFactory.CreateOutput(Arg.Any<IMessageLogger>()).Returns(outputLogger);

            discoverer.DiscoverTests(
                sources,
                autoSubstitute.Resolve<IDiscoveryContext>(),
                messageLogger,
                discoverySink);
        }

        [Test]
        public void it_should_pass_message_logger()
        {
            binaryTestDiscoverer.Received().Discover(
                Arg.Any<string>(), outputLogger, outputLogger);
        }

        [Test]
        public void it_should_send_discovered_test_cases()
        {
            var allSpecs = specificationBySource.SelectMany(group => group.Value);

            var expected = allSpecs.Select(spec => spec.FullName);

            var actual = testCases.Select(tc => tc.FullyQualifiedName);

            actual.Should().BeEquivalentTo(expected);
        }
    }
}
