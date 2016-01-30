using AutofacContrib.NSubstitute;
using FluentAssertions;
using NSpec.VsAdapter.Discovery;
using NSpec.VsAdapter.Logging;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.UnitTests.Discovery
{
    [TestFixture]
    [Category("BinaryTestDiscoverer")]
    public abstract class BinaryTestDiscoverer_desc_base
    {
        protected BinaryTestDiscoverer discoverer;

        protected AutoSubstitute autoSubstitute;
        protected ICrossDomainCollector crossDomainCollector;
        protected IOutputLogger logger;
        protected IReplayLogger replayLogger;

        protected const string somePath = @".\path\to\some\dummy-library.dll";

        [SetUp]
        public virtual void before_each()
        {
            autoSubstitute = new AutoSubstitute();

            crossDomainCollector = autoSubstitute.Resolve<ICrossDomainCollector>();

            logger = autoSubstitute.Resolve<IOutputLogger>();
            replayLogger = autoSubstitute.Resolve<IReplayLogger>();

            discoverer = autoSubstitute.Resolve<BinaryTestDiscoverer>();
        }

        [TearDown]
        public virtual void after_each()
        {
            autoSubstitute.Dispose();
        }
    }

    public class BinaryTestDiscoverer_when_discovery_succeeds : BinaryTestDiscoverer_desc_base
    {
        readonly static DiscoveredExample[] someDiscoveredExamples = new DiscoveredExample[] 
        { 
            new DiscoveredExample() { SourceFilePath = somePath, FullName = "source-1-spec-A", },
            new DiscoveredExample() { SourceFilePath = somePath, FullName = "source-1-spec-B", },
            new DiscoveredExample() { SourceFilePath = somePath, FullName = "source-1-spec-C", },
        };

        public override void before_each()
        {
            base.before_each();

            crossDomainCollector.Run(null, null).ReturnsForAnyArgs(callInfo =>
                {
                    string binaryPath = callInfo.Arg<string>();

                    return (binaryPath == somePath ? someDiscoveredExamples : new DiscoveredExample[0]);
                });
        }

        [Test]
        public void it_should_return_collected_specifications()
        {
            discoverer.Discover(somePath, logger, replayLogger).Should().BeEquivalentTo(someDiscoveredExamples);
        }
    }

    public class BinaryTestDiscoverer_when_discovery_fails : BinaryTestDiscoverer_desc_base
    {
        IEnumerable<DiscoveredExample> discoveredExamples;

        public override void before_each()
        {
            base.before_each();

            crossDomainCollector.Run(null, null).ReturnsForAnyArgs(_ =>
                {
                    throw new DummyTestException();
                });

            discoveredExamples = discoverer.Discover(somePath, logger, replayLogger);
        }

        [Test]
        public void it_should_return_empty_spec_list()
        {
            discoveredExamples.Should().BeEmpty();
        }

        [Test]
        public void it_should_log_error_and_exception()
        {
            logger.Received(1).Error(Arg.Any<DummyTestException>(), Arg.Any<string>());
        }
    }
}
