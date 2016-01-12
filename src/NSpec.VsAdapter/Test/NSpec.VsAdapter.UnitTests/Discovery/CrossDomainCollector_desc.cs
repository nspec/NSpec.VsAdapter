using AutofacContrib.NSubstitute;
using FluentAssertions;
using NSpec.VsAdapter.CrossDomain;
using NSpec.VsAdapter.Discovery;
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
    [Category("CrossDomainCollector")]
    public abstract class CrossDomainCollector_desc_base
    {
        protected CrossDomainCollector collector;

        protected AutoSubstitute autoSubstitute;
        protected IAppDomainFactory appDomainFactory;
        protected IMarshalingFactory<IEnumerable<NSpecSpecification>> marshalingFactory;
        protected MarshalingProxy<IEnumerable<NSpecSpecification>> crossDomainProxy;
        protected ITargetAppDomain targetDomain;
        protected IOutputLogger logger;
        protected Func<IEnumerable<NSpecSpecification>> targetOperation;
        protected IEnumerable<NSpecSpecification> actualSpecifications;

        protected const string somePath = @".\some\path\to\library.dll";

        [SetUp]
        public virtual void before_each()
        {
            autoSubstitute = new AutoSubstitute();

            appDomainFactory = autoSubstitute.Resolve<IAppDomainFactory>();

            marshalingFactory = autoSubstitute
                .Resolve<IMarshalingFactory<IEnumerable<NSpecSpecification>>>();

            crossDomainProxy = Substitute.For<MarshalingProxy<IEnumerable<NSpecSpecification>>>();

            targetDomain = Substitute.For<ITargetAppDomain>();

            collector = autoSubstitute.Resolve<CrossDomainCollector>();

            logger = autoSubstitute.Resolve<IOutputLogger>();

            targetOperation = () => null;
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

            crossDomainProxy.Execute(targetOperation).Returns(someSpecifications);

            actualSpecifications = collector.Run(somePath, targetOperation);
        }

        [Test]
        public void it_should_return_collected_specifications()
        {
            actualSpecifications.Should().BeEquivalentTo(someSpecifications);
        }

        [Test]
        public void it_should_dispose_target_app_domain()
        {
            targetDomain.Received(1).Dispose();
        }

        [Test]
        public void it_should_dispose_marshaling_proxy()
        {
            crossDomainProxy.Received(1).Dispose();
        }
    }

    public abstract class CrossDomainCollector_when_run_fails : CrossDomainCollector_desc_base
    {
        [Test]
        [ExpectedException(typeof(DummyTestException))]
        public void it_should_let_exception_flow()
        {
            actualSpecifications = collector.Run(somePath, targetOperation);
        }
    }

    public class CrossDomainCollector_when_app_domain_creation_fails : CrossDomainCollector_when_run_fails
    {
        public override void before_each()
        {
            base.before_each();

            appDomainFactory.Create(null).ReturnsForAnyArgs(_ =>
            {
                throw new DummyTestException();
            });
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
                throw new DummyTestException();
            });
        }

        [Test]
        public void it_should_dispose_target_app_domain()
        {
            try
            {
                actualSpecifications = collector.Run(somePath, targetOperation);
            }
            catch (Exception)
            {
            }

            targetDomain.Received(1).Dispose();
        }
    }

    public class CrossDomainCollector_when_marshaled_execution_fails : CrossDomainCollector_when_run_fails
    {
        public override void before_each()
        {
            base.before_each();

            appDomainFactory.Create(null).ReturnsForAnyArgs(targetDomain);

            marshalingFactory.CreateProxy(null).ReturnsForAnyArgs(crossDomainProxy);

            crossDomainProxy.Execute(null).ReturnsForAnyArgs(_ =>
            {
                throw new DummyTestException();
            });
        }

        [Test]
        public void it_should_dispose_target_app_domain()
        {
            try
            {
                actualSpecifications = collector.Run(somePath, targetOperation);
            }
            catch (Exception)
            {
            }

            targetDomain.Received(1).Dispose();
        }

        [Test]
        public void it_should_dispose_marshaling_proxy()
        {
            try
            {
                actualSpecifications = collector.Run(somePath, targetOperation);
            }
            catch (Exception)
            {
            }

            crossDomainProxy.Received(1).Dispose();
        }
    }
}
