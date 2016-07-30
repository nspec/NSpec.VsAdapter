using AutofacContrib.NSubstitute;
using FluentAssertions;
using NSpec.VsAdapter.Common;
using NSpec.VsAdapter.Core.CrossDomain;
using NSpec.VsAdapter.Core.Discovery;
using NSpec.VsAdapter.Core.Discovery.Target;
using NSpec.VsAdapter.Logging;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace NSpec.VsAdapter.UnitTests.Core.Discovery
{
    [TestFixture]
    [Category("BinaryTestDiscoverer")]
    public abstract class BinaryTestDiscoverer_desc_base
    {
        protected BinaryTestDiscoverer discoverer;

        protected AutoSubstitute autoSubstitute;
        protected ICrossDomainRunner<IProxyableTestDiscoverer, DiscoveredExample[]> remoteRunner;
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

            proxyableDiscoverer = Substitute.For<IProxyableTestDiscoverer>();

            remoteRunner = autoSubstitute.SubstituteFor<
                ICrossDomainRunner<IProxyableTestDiscoverer, DiscoveredExample[]>>();

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

            remoteRunner
                .Run(somePath,
                    Arg.Any<Func<IProxyableTestDiscoverer, DiscoveredExample[]>>(),
                    Arg.Any<Func<Exception, string, DiscoveredExample[]>>())
                .Returns(callInfo =>
                {
                    var path = callInfo.Arg<string>();
                    var operation = callInfo.Arg<Func<IProxyableTestDiscoverer, DiscoveredExample[]>>();

                    return operation(proxyableDiscoverer);
                });

            actuals = discoverer.Discover(somePath, logger, crossDomainLogger);
        }

        [Test]
        public void it_should_return_collected_specifications()
        {
            var expecteds = someDiscoveredExamples;

            actuals.Should().BeEquivalentTo(expecteds);
        }
    }

    public class BinaryTestDiscoverer_when_discovery_fails : BinaryTestDiscoverer_when_nspec_found
    {
        DummyTestException expectedEx;

        public override void before_each()
        {
            base.before_each();

            expectedEx = new DummyTestException();

            remoteRunner
                .Run(somePath,
                    Arg.Any<Func<IProxyableTestDiscoverer, DiscoveredExample[]>>(),
                    Arg.Any<Func<Exception, string, DiscoveredExample[]>>())
                .Returns(callInfo =>
                {
                    var path = callInfo.Arg<string>();
                    var fail = callInfo.Arg<Func<Exception, string, DiscoveredExample[]>>();

                    return fail(expectedEx, path);
                });

            actuals = discoverer.Discover(somePath, logger, crossDomainLogger);
        }

        [Test]
        public void it_should_return_empty_spec_list()
        {
            actuals.Should().BeEmpty();
        }

        [Test]
        public void it_should_log_error_with_exception()
        {
            logger.Received(1).Error(expectedEx, Arg.Any<string>());
        }
    }

    public class BinaryTestDiscoverer_when_nspec_not_found : BinaryTestDiscoverer_desc_base
    {
        public override void before_each()
        {
            base.before_each();

            fileService.Exists(nspecPath).Returns(false);

            var unexpectedExamples = someDiscoveredExamples;

            remoteRunner
                .Run(Arg.Any<string>(),
                    Arg.Any<Func<IProxyableTestDiscoverer, DiscoveredExample[]>>(),
                    Arg.Any<Func<Exception, string, DiscoveredExample[]>>())
                .Returns(_ =>
                {
                    return unexpectedExamples;
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
