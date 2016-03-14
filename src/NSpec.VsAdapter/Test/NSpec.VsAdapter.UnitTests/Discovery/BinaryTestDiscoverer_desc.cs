using AutofacContrib.NSubstitute;
using FluentAssertions;
using NSpec.VsAdapter.CrossDomain;
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
        protected IAppDomainFactory appDomainFactory;
        protected ITargetAppDomain targetDomain;
        protected IProxyableFactory<IProxyableTestDiscoverer> proxyableFactory;
        protected IProxyableTestDiscoverer proxyableDiscoverer;
        protected IFileService fileService;
        protected IOutputLogger logger;
        protected ICrossDomainLogger crossDomainLogger;

        protected IEnumerable<DiscoveredExample> actuals;

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

            targetDomain = Substitute.For<ITargetAppDomain>();
            appDomainFactory = autoSubstitute.Resolve<IAppDomainFactory>();
            appDomainFactory.Create(somePath).Returns(targetDomain);

            proxyableDiscoverer = Substitute.For<IProxyableTestDiscoverer>();
            proxyableFactory = autoSubstitute.Resolve<IProxyableFactory<IProxyableTestDiscoverer>>();
            proxyableFactory.CreateProxy(targetDomain).Returns(proxyableDiscoverer);

            fileService = autoSubstitute.Resolve<IFileService>();

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

            proxyableDiscoverer.Discover(somePath, crossDomainLogger).Returns(someDiscoveredExamples);

            actuals = discoverer.Discover(somePath, logger, crossDomainLogger);
        }

        [Test]
        public void it_should_return_collected_specifications()
        {
            var expecteds = someDiscoveredExamples;

            actuals.Should().BeEquivalentTo(expecteds);
        }
    }

    public abstract class BinaryTestDiscoverer_when_discovery_fails : BinaryTestDiscoverer_when_nspec_found
    {
        protected DummyTestException ex;

        [Test]
        public void it_should_return_empty_spec_list()
        {
            actuals.Should().BeEmpty();
        }

        [Test]
        public void it_should_log_error_and_exception()
        {
            logger.Received(1).Error(ex, Arg.Any<string>());
        }
    }

    public class BinaryTestDiscoverer_when_creating_appdomain_fails : BinaryTestDiscoverer_when_discovery_fails
    {
        public override void before_each()
        {
            base.before_each();

            appDomainFactory.Create(somePath).Returns(_ =>
            {
                ex = new DummyTestException();
                throw ex;
            });

            actuals = discoverer.Discover(somePath, logger, crossDomainLogger);
        }
    }

    public class BinaryTestDiscoverer_when_creating_proxyable_fails : BinaryTestDiscoverer_when_discovery_fails
    {
        public override void before_each()
        {
            base.before_each();

            proxyableFactory.CreateProxy(targetDomain).Returns(_ =>
            {
                ex = new DummyTestException();
                throw ex;
            });

            actuals = discoverer.Discover(somePath, logger, crossDomainLogger);
        }
    }

    public class BinaryTestDiscoverer_when_discovering_locally_fails : BinaryTestDiscoverer_when_discovery_fails
    {
        public override void before_each()
        {
            base.before_each();

            proxyableDiscoverer.Discover(somePath, crossDomainLogger).Returns(_ =>
            {
                ex = new DummyTestException();
                throw ex;
            });

            actuals = discoverer.Discover(somePath, logger, crossDomainLogger);
        }
    }

    public class BinaryTestDiscoverer_when_nspec_not_found : BinaryTestDiscoverer_desc_base
    {
        public override void before_each()
        {
            base.before_each();

            fileService.Exists(nspecPath).Returns(false);

            proxyableDiscoverer.Discover(null, null).ReturnsForAnyArgs(_ =>
            {
                throw new DummyTestException();
            });

            actuals = discoverer.Discover(somePath, logger, crossDomainLogger);
        }

        [Test]
        public void it_should_skip_binary()
        {
            actuals.Should().BeEmpty();
        }
    }
}
