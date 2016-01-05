using AutofacContrib.NSubstitute;
using FluentAssertions;
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
    [Category("CrossDomainCollector")]
    [Ignore("Test yet to be implemented")]
    public abstract class CrossDomainCollector_desc_base
    {
        protected CrossDomainCollector collector;

        protected AutoSubstitute autoSubstitute;
        protected IAppDomainFactory appDomainFactory;
        protected IMarshalingFactory<ICollectorInvocation, IEnumerable<NSpecSpecification>> marshalingFactory;
        protected MarshalingProxy<ICollectorInvocation, IEnumerable<NSpecSpecification>> crossDomainProxy;
        protected ITargetAppDomain targetDomain;
        protected ICollectorInvocation collectorInvocation;
        protected Func<ICollectorInvocation, IEnumerable<NSpecSpecification>> targetOperation;
        protected IEnumerable<NSpecSpecification> actualSpecifications;

        protected const string somePath = @".\some\path\to\library.dll";

        [SetUp]
        public virtual void before_each()
        {
            autoSubstitute = new AutoSubstitute();

            appDomainFactory = autoSubstitute.Resolve<IAppDomainFactory>();

            marshalingFactory = autoSubstitute
                .Resolve<IMarshalingFactory<ICollectorInvocation, IEnumerable<NSpecSpecification>>>();

            crossDomainProxy = Substitute.For<MarshalingProxy<ICollectorInvocation, IEnumerable<NSpecSpecification>>>();

            targetDomain = Substitute.For<ITargetAppDomain>();

            collector = autoSubstitute.Resolve<CrossDomainCollector>();

            collectorInvocation = Substitute.For<ICollectorInvocation>();

            targetOperation = _ => null;
        }

        [TearDown]
        public virtual void after_each()
        {
            autoSubstitute.Dispose();
        }
    }

    public class CrossDomainCollector_when_run_succeeds : CrossDomainCollector_desc_base
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

            appDomainFactory.Create(somePath).Returns(targetDomain);

            marshalingFactory.CreateProxy(targetDomain).Returns(crossDomainProxy);

            crossDomainProxy.Execute(collectorInvocation, targetOperation).Returns(someSpecifications);

            actualSpecifications = collector.Run(somePath, collectorInvocation, targetOperation);
        }

        [Test]
        public void it_should_return_collected_specifications()
        {
            actualSpecifications.Should().BeEquivalentTo(someSpecifications);
        }
    }

    public abstract class CrossDomainCollector_when_run_fails : CrossDomainCollector_desc_base
    {
        [Test]
        public void it_should_return_empty_spec_list()
        {
            actualSpecifications.Should().BeEmpty();
        }
    }

    public class CrossDomainCollector_when_app_domain_creation_fails : CrossDomainCollector_when_run_fails
    {
        public override void before_each()
        {
            base.before_each();

            appDomainFactory.Create(null).ReturnsForAnyArgs(_ =>
            {
                throw new InvalidOperationException();
            });

            actualSpecifications = collector.Run(somePath, collectorInvocation, targetOperation);
        }
    }

    public class CrossDomainCollector_when_marshal_wrapper_creation_fails : CrossDomainCollector_when_run_fails
    {
        public override void before_each()
        {
            base.before_each();

            appDomainFactory.Create(null).ReturnsForAnyArgs(targetDomain);

            marshalingFactory.CreateProxy(null).ReturnsForAnyArgs(_ =>
            {
                throw new InvalidOperationException();
            });

            actualSpecifications = collector.Run(somePath, collectorInvocation, targetOperation);
        }
    }

    public class CrossDomainCollector_when_marshaled_execution_fails : CrossDomainCollector_when_run_fails
    {
        public override void before_each()
        {
            base.before_each();

            appDomainFactory.Create(null).ReturnsForAnyArgs(targetDomain);

            marshalingFactory.CreateProxy(null).ReturnsForAnyArgs(crossDomainProxy);

            crossDomainProxy.Execute(null, null).ReturnsForAnyArgs(_ =>
            {
                throw new InvalidOperationException();
            });

            actualSpecifications = collector.Run(somePath, collectorInvocation, targetOperation);
        }
    }
}
