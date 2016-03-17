using AutofacContrib.NSubstitute;
using FluentAssertions;
using NSpec.Domain;
using NSpec.VsAdapter.Discovery;
using NSpec.VsAdapter.Discovery.Target;
using NSpec.VsAdapter.Logging;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.UnitTests.Discovery.Target
{
    [TestFixture]
    [Category("ProxyableTestDiscoverer")]
    public abstract class ProxyableTestDiscoverer_desc_base
    {
        protected ProxyableTestDiscoverer discoverer;
        
        protected AutoSubstitute autoSubstitute;
        protected IExampleFinder exampleFinder;
        protected IDebugInfoProviderFactory debugInfoProviderFactory;
        protected IDebugInfoProvider debugInfoProvider;
        protected IDiscoveredExampleMapper exampleMapper;
        protected ICrossDomainLogger crossDomainLogger;
        
        protected DiscoveredExample[] actuals;

        protected readonly ExampleBase[] someFoundExamples = new ExampleBase[]
        {
            new Example("source-1-spec-A", "tag-1-A", () => {}, false),
            new Example("source-1-spec-B", "tag-1-B", () => {}, false),
            new Example("source-1-spec-C", "tag-1-C", () => {}, true),
        };

        protected readonly DiscoveredExample[] someDiscoveredExamples = new DiscoveredExample[] 
        { 
            new DiscoveredExample() { SourceFilePath = somePath, FullName = "source-1-spec-A", },
            new DiscoveredExample() { SourceFilePath = somePath, FullName = "source-1-spec-B", },
            new DiscoveredExample() { SourceFilePath = somePath, FullName = "source-1-spec-C", },
        };

        protected const string somePath = @".\path\to\some\dummy-library.dll";

        [SetUp]
        public virtual void before_each()
        {
            autoSubstitute = new AutoSubstitute();

            exampleFinder = autoSubstitute.Resolve<IExampleFinder>();
            debugInfoProviderFactory = autoSubstitute.Resolve<IDebugInfoProviderFactory>();
            var discoveredExampleMapperFactory = autoSubstitute.Resolve<IDiscoveredExampleMapperFactory>();

            crossDomainLogger = Substitute.For<ICrossDomainLogger>();

            debugInfoProvider = Substitute.For<IDebugInfoProvider>();
            debugInfoProviderFactory.Create(somePath, crossDomainLogger).Returns(debugInfoProvider);

            exampleMapper = Substitute.For<IDiscoveredExampleMapper>();
            discoveredExampleMapperFactory.Create(somePath, debugInfoProvider).Returns(exampleMapper);

            exampleFinder.Find(somePath).Returns(someFoundExamples);

            var exampleTuples = someFoundExamples.Zip(someDiscoveredExamples, 
                (found, discovered) => new Tuple<ExampleBase, DiscoveredExample>(found, discovered));

            foreach (var tuple in exampleTuples)
            {
                var found = tuple.Item1;
                var discovered = tuple.Item2;

                exampleMapper.FromExample(found).Returns(discovered);
            }

            discoverer = autoSubstitute.Resolve<ProxyableTestDiscoverer>();
        }

        [TearDown]
        public virtual void after_each()
        {
            autoSubstitute.Dispose();
            discoverer.Dispose();
        }

        protected void act_each()
        {
            actuals = discoverer.Discover(somePath, crossDomainLogger);
        }
    }

    public class ProxyableTestDiscoverer_when_discover_succeeds : ProxyableTestDiscoverer_desc_base
    {
        public override void before_each()
        {
            base.before_each();

            act_each();
        }

        [Test]
        public void it_should_return_discovered_examples()
        {
            var expecteds = someDiscoveredExamples;

            actuals.Should().BeEquivalentTo(expecteds);
        }
    }

    public abstract class ProxyableTestDiscoverer_when_discover_fails : ProxyableTestDiscoverer_desc_base
    {
        protected DummyTestException expectedEx;

        public override void before_each()
        {
            base.before_each();

            expectedEx = new DummyTestException();
        }

        [Test]
        public void it_should_return_empty_example_list()
        {
            actuals.Should().BeEmpty();
        }

        [Test]
        public void it_should_log_error_with_exception()
        {
            var expectedInfo = new ExceptionLogInfo(expectedEx);

            crossDomainLogger.Received(1).Error(
                Arg.Is<ExceptionLogInfo>(actualInfo => MatchExceptionLogInfo(actualInfo, expectedInfo)), 
                Arg.Any<string>());
        }

        static bool MatchExceptionLogInfo(ExceptionLogInfo actualInfo, ExceptionLogInfo expectedInfo)
        {
            return actualInfo.Type == expectedInfo.Type &&
                actualInfo.Content == expectedInfo.Content;
        }
    }

    public class ProxyableTestDiscoverer_when_finding_examples_fails : ProxyableTestDiscoverer_when_discover_fails
    {
        public override void before_each()
        {
            base.before_each();

            exampleFinder.Find(somePath).Returns(_ =>
            {
                throw expectedEx;
            });

            act_each();
        }
    }

    public class ProxyableTestDiscoverer_when_debug_info_creation_fails : ProxyableTestDiscoverer_when_discover_fails
    {
        public override void before_each()
        {
            base.before_each();

            debugInfoProviderFactory.Create(somePath, crossDomainLogger).Returns(_ =>
            {
                throw expectedEx;
            });

            act_each();
        }
    }

    public class ProxyableTestDiscoverer_when_example_mapping_fails : ProxyableTestDiscoverer_when_discover_fails
    {
        public override void before_each()
        {
            base.before_each();

            exampleMapper.FromExample(someFoundExamples[2]).Returns(_ =>
            {
                throw expectedEx;
            });

            act_each();
        }
    }
}
