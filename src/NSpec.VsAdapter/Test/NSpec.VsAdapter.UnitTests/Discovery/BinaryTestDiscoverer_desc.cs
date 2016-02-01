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
        protected IFileService fileService;
        protected ICrossDomainCollector crossDomainCollector;
        protected IOutputLogger logger;
        protected ICrossDomainLogger crossDomainLogger;

        protected readonly DiscoveredExample[] someDiscoveredExamples = new DiscoveredExample[] 
        { 
            new DiscoveredExample() { SourceFilePath = somePath, FullName = "source-1-spec-A", },
            new DiscoveredExample() { SourceFilePath = somePath, FullName = "source-1-spec-B", },
            new DiscoveredExample() { SourceFilePath = somePath, FullName = "source-1-spec-C", },
        };

        protected const string somePath = @".\path\to\some\dummy-library.dll";
        protected const string nspecPath = @".\path\to\some\nspec.dll";

        [SetUp]
        public virtual void before_each()
        {
            autoSubstitute = new AutoSubstitute();

            fileService = autoSubstitute.Resolve<IFileService>();
            crossDomainCollector = autoSubstitute.Resolve<ICrossDomainCollector>();

            logger = autoSubstitute.Resolve<IOutputLogger>();
            crossDomainLogger = autoSubstitute.Resolve<ICrossDomainLogger>();

            discoverer = autoSubstitute.Resolve<BinaryTestDiscoverer>();
        }

        [TearDown]
        public virtual void after_each()
        {
            autoSubstitute.Dispose();
        }
    }

    public abstract class BinaryTestDiscoverer_when_nspec_found : BinaryTestDiscoverer_desc_base
    {
        public override void before_each()
        {
            base.before_each();

            fileService.Exists(nspecPath).Returns(true);
        }
    }

    public class BinaryTestDiscoverer_when_discovery_succeeds : BinaryTestDiscoverer_when_nspec_found
    {
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
            discoverer.Discover(somePath, logger, crossDomainLogger).Should().BeEquivalentTo(someDiscoveredExamples);
        }
    }

    public class BinaryTestDiscoverer_when_discovery_fails : BinaryTestDiscoverer_when_nspec_found
    {
        IEnumerable<DiscoveredExample> discoveredExamples;

        public override void before_each()
        {
            base.before_each();

            crossDomainCollector.Run(null, null).ReturnsForAnyArgs(_ =>
                {
                    throw new DummyTestException();
                });

            discoveredExamples = discoverer.Discover(somePath, logger, crossDomainLogger);
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

    public class BinaryTestDiscoverer_when_nspec_not_found : BinaryTestDiscoverer_desc_base
    {
        public override void before_each()
        {
            base.before_each();

            fileService.Exists(nspecPath).Returns(false);

            crossDomainCollector.Run(null, null).ReturnsForAnyArgs(callInfo =>
            {
                string binaryPath = callInfo.Arg<string>();

                return (binaryPath == somePath ? someDiscoveredExamples : new DiscoveredExample[0]);
            });
        }

        [Test]
        public void it_should_skip_binary()
        {
            discoverer.Discover(somePath, logger, crossDomainLogger).Should().BeEmpty();
        }
    }
}
