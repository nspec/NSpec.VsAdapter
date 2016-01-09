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
        IMessageLogger messageLogger;
        string[] sources;

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

            crossDomainTestDiscoverer.Discover(null, null, null).ReturnsForAnyArgs(callInfo =>
                {
                    string assemblyPath = callInfo.Arg<string>();

                    if (assemblyPath == source1 || assemblyPath == source2)
                    {
                        return groupedSpecifications[assemblyPath];
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

            messageLogger = autoSubstitute.Resolve<IMessageLogger>();

            discoverer.DiscoverTests(
                sources,
                autoSubstitute.Resolve<IDiscoveryContext>(),
                messageLogger,
                discoverySink);
        }

        [Test]
        public void it_should_pass_message_logger()
        {
            crossDomainTestDiscoverer.Received().Discover(
                Arg.Any<string>(), 
                Arg.Is<OutputLogger>(ol => ol.MessageLogger == messageLogger),
                Arg.Is<OutputLogger>(ol => ol.MessageLogger == messageLogger));
        }

        [Test]
        public void it_should_send_discovered_test_cases()
        {
            throw new NotImplementedException();
            // TODO
        }
    }
}
