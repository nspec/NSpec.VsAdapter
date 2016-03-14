using AutofacContrib.NSubstitute;
using FluentAssertions;
using NSpec.VsAdapter.CrossDomain;
using NSpec.VsAdapter.Logging;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// TODO check for deletion
#if false
namespace NSpec.VsAdapter.UnitTests.CrossDomain
{
    [TestFixture]
    public abstract class CrossDomainRunner_desc_base<T>
    {
        protected AutoSubstitute autoSubstitute;
        protected IAppDomainFactory appDomainFactory;
        protected IProxyFactory<T> proxyFactory;
        protected Proxy<T> crossDomainProxy;
        protected ITargetAppDomain targetDomain;
        protected IOutputLogger logger;
        protected Func<T> targetOperation;

        protected const string somePath = @".\some\path\to\library.dll";

        [SetUp]
        public virtual void before_each()
        {
            autoSubstitute = new AutoSubstitute();

            appDomainFactory = autoSubstitute.Resolve<IAppDomainFactory>();

            proxyFactory = autoSubstitute
                .Resolve<IProxyFactory<T>>();

            crossDomainProxy = Substitute.For<Proxy<T>>();

            targetDomain = Substitute.For<ITargetAppDomain>();

            logger = autoSubstitute.Resolve<IOutputLogger>();

            targetOperation = () => default(T);
        }

        [TearDown]
        public virtual void after_each()
        {
            autoSubstitute.Dispose();
        }
    }

    public abstract class CrossDomainRunner_when_run_succeeds<T> : CrossDomainRunner_desc_base<T>
    {
        public override void before_each()
        {
            base.before_each();

            appDomainFactory.Create(somePath).Returns(targetDomain);

            proxyFactory.CreateProxy(targetDomain).Returns(crossDomainProxy);
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

    public abstract class CrossDomainRunner_when_run_fails<T> : CrossDomainRunner_desc_base<T>
    {
        protected abstract void CreateRunner();
        protected abstract void ExerciseRunner();

        public override void before_each()
        {
            base.before_each();

            CreateRunner();
        }

        [Test]
        [ExpectedException(typeof(DummyTestException))]
        public void it_should_let_exception_flow()
        {
            ExerciseRunner();
        }
    }

    public abstract class CrossDomainRunner_when_marshal_wrapper_creation_fails<T> : CrossDomainRunner_when_run_fails<T>
    {
        public override void before_each()
        {
            base.before_each();

            appDomainFactory.Create(null).ReturnsForAnyArgs(targetDomain);

            proxyFactory.CreateProxy(null).ReturnsForAnyArgs(_ =>
            {
                throw new DummyTestException();
            });
        }

        [Test]
        public void it_should_dispose_target_app_domain()
        {
            try
            {
                ExerciseRunner();
            }
            catch (Exception)
            {
            }

            targetDomain.Received(1).Dispose();
        }
    }

    public abstract class CrossDomainRunner_when_marshaled_execution_fails<T> : CrossDomainRunner_when_run_fails<T>
    {
        public override void before_each()
        {
            base.before_each();

            appDomainFactory.Create(null).ReturnsForAnyArgs(targetDomain);

            proxyFactory.CreateProxy(null).ReturnsForAnyArgs(crossDomainProxy);

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
                ExerciseRunner();
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
                ExerciseRunner();
            }
            catch (Exception)
            {
            }

            crossDomainProxy.Received(1).Dispose();
        }
    }
}
#endif
