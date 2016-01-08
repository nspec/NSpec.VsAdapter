using AutofacContrib.NSubstitute;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using NSpec.VsAdapter.NSpecModding;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.UnitTests.NSpecModding
{
    [TestFixture]
    [Category("CrossDomainTestDiscoverer")]
    public abstract class CrossDomainTestDiscoverer_desc_base
    {
        protected CrossDomainTestDiscoverer discoverer;

        protected AutoSubstitute autoSubstitute;
        protected IOutputLogger logger;
        protected ICrossDomainCollector crossDomainCollector;

        protected const string somePath = @".\some\path\to\library.dll";

        [SetUp]
        public virtual void before_each()
        {
            autoSubstitute = new AutoSubstitute();

            logger = autoSubstitute.Resolve<IOutputLogger>();

            crossDomainCollector = autoSubstitute.Resolve<ICrossDomainCollector>();

            discoverer = autoSubstitute.Resolve<CrossDomainTestDiscoverer>();
        }

        [TearDown]
        public virtual void after_each()
        {
            autoSubstitute.Dispose();
        }
    }

    public class CrossDomainTestDiscoverer_when_discovery_succeeds : CrossDomainTestDiscoverer_desc_base
    {
        readonly static NSpecSpecification[] someSpecifications = new NSpecSpecification[] 
        { 
            new NSpecSpecification() { SourceFilePath = somePath, FullName = "source-1-spec-A", },
            new NSpecSpecification() { SourceFilePath = somePath, FullName = "source-1-spec-B", },
            new NSpecSpecification() { SourceFilePath = somePath, FullName = "source-1-spec-C", },
        };

        public override void before_each()
        {
            base.before_each();

            crossDomainCollector.Run(null, null).ReturnsForAnyArgs(callInfo =>
                {
                    string assemblyPath = callInfo.Arg<string>();

                    return (assemblyPath == somePath ? someSpecifications : new NSpecSpecification[0]);
                });
        }

        [Test]
        public void it_should_return_collected_specifications()
        {
            discoverer.Discover(somePath, logger).Should().BeEquivalentTo(someSpecifications);
        }
    }

    public class CrossDomainTestDiscoverer_when_discovery_fails : CrossDomainTestDiscoverer_desc_base
    {
        IEnumerable<NSpecSpecification> specifications;

        public override void before_each()
        {
            base.before_each();

            crossDomainCollector.Run(null, null).ReturnsForAnyArgs(_ =>
                {
                    throw new InvalidOperationException();
                });

            specifications = discoverer.Discover(somePath, logger);
        }

        [Test]
        public void it_should_return_empty_spec_list()
        {
            specifications.Should().BeEmpty();
        }

        [Test]
        public void it_should_log_error_and_exception()
        {
            logger.Received(1).Error(Arg.Any<InvalidOperationException>(), Arg.Any<string>());
        }
    }
}
