using AutofacContrib.NSubstitute;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using NSpec.VsAdapter.NSpecModding;
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
        protected ICrossDomainTestDiscoverer crossDomainTestDiscoverer;

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

            crossDomainTestDiscoverer = autoSubstitute.Resolve<ICrossDomainTestDiscoverer>();

            discoverer = autoSubstitute.Resolve<NSpecTestDiscoverer>();
        }

        [TearDown]
        public virtual void after_each()
        {
            autoSubstitute.Dispose();
            testCases.Clear();
        }
    }

    public class NSpecTestDiscoverer_when_discovering : NSpecTestDiscoverer_desc_base
    {
        IMessageLogger logger;
        string[] sources;
        List<NSpecSpecification> flatSpecifications;

        public override void before_each()
        {
            base.before_each();

            string source1 = @".\some\dummy\some-library.dll";
            string source2 = @".\another\dummy\another-library.dll";

            sources = new string[]
            {
                source1,
                source2,
            };

            var groupedSpecifications = new Dictionary<string, NSpecSpecification[]>()
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

            flatSpecifications = groupedSpecifications.SelectMany(collection => collection.Value).ToList();

            crossDomainTestDiscoverer.Discover(Arg.Any<string>(), Arg.Any<IMessageLogger>()).Returns(new NSpecSpecification[0]);
            crossDomainTestDiscoverer.Discover(source1, Arg.Any<IMessageLogger>()).Returns(groupedSpecifications[source1]);
            crossDomainTestDiscoverer.Discover(source2, Arg.Any<IMessageLogger>()).Returns(groupedSpecifications[source2]);

            logger = autoSubstitute.Resolve<IMessageLogger>();

            discoverer.DiscoverTests(
                sources,
                autoSubstitute.Resolve<IDiscoveryContext>(),
                logger,
                discoverySink);
        }

        [Test]
        public void it_should_pass_message_logger()
        {
            crossDomainTestDiscoverer.Received().Discover(Arg.Any<string>(), logger);
        }

        [Test]
        public void it_should_send_discovered_source_paths()
        {
            var sourcePaths = testCases.Select(testCase => testCase.Source).Distinct().ToList();

            sourcePaths.ShouldBeEquivalentTo(sources);
        }

        [Test]
        public void it_should_send_discovered_fullnames()
        {
            var fullNames = flatSpecifications.Select(spec => spec.FullName);

            testCases.Select(testCase => testCase.FullyQualifiedName).Distinct().ShouldBeEquivalentTo(fullNames);
        }
    }
}
