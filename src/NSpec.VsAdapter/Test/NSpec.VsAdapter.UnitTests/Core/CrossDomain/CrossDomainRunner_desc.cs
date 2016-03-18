using AutofacContrib.NSubstitute;
using FluentAssertions;
using NSpec.VsAdapter.Core.CrossDomain;
using NSpec.VsAdapter.Logging;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.UnitTests.Core.CrossDomain
{
    [TestFixture]
    [Category("CrossDomainRunner")]
    public class CrossDomainRunner_desc_base
    {
        protected CrossDomainRunner<IDummyProxyable, float> runner;

        protected AutoSubstitute autoSubstitute;
        protected IAppDomainFactory appDomainFactory;
        protected ITargetAppDomain targetDomain;
        protected IProxyableFactory<IDummyProxyable> proxyableFactory;
        protected IDummyProxyable proxyable;

        protected const string somePath = @".\some\path\to\library.dll";

        [SetUp]
        public virtual void before_each()
        {
            autoSubstitute = new AutoSubstitute();

            targetDomain = Substitute.For<ITargetAppDomain>();
            appDomainFactory = autoSubstitute.Resolve<IAppDomainFactory>();
            appDomainFactory.Create(somePath).Returns(targetDomain);

            proxyable = Substitute.For<IDummyProxyable>();
            proxyableFactory = autoSubstitute.Resolve<IProxyableFactory<IDummyProxyable>>();
            proxyableFactory.CreateProxy(targetDomain).Returns(proxyable);

            runner = autoSubstitute.Resolve<CrossDomainRunner<IDummyProxyable, float>>();

            proxyable.PassInput(Arg.Any<float>()).Returns(callInfo =>
            {
                float input = callInfo.Arg<float>();
                return input;
            });
        }

        [TearDown]
        public virtual void after_each()
        {
            autoSubstitute.Dispose();
        }

        public interface IDummyProxyable : IDisposable
        {
            float PassInput(float input);
        }
    }

    public class CrossDomainRunner_when_run_succeeds : CrossDomainRunner_desc_base
    {
        float actual;
        
        protected const float expected = 456F;

        float RemoteOperation(IDummyProxyable someProxyable)
        {
            return someProxyable.PassInput(expected);
        }

        float FailCallback(Exception ex, string path)
        {
            throw new DummyTestException("It should not pass by here");
        }

        public override void before_each()
        {
            base.before_each();

            actual = runner.Run(somePath, RemoteOperation, FailCallback);
        }

        [Test]
        public void it_should_return_result()
        {
            actual.Should().Be(expected);
        }

        [Test]
        public void it_should_dispose_target_app_domain()
        {
            targetDomain.Received(1).Dispose();
        }

        [Test]
        public void it_should_dispose_proxyable_executor()
        {
            proxyable.Received(1).Dispose();
        }
    }

    public abstract class CrossDomainRunner_when_run_fails : CrossDomainRunner_desc_base
    {
        protected float actual;
        protected Exception actualEx;
        protected DummyTestException expectedEx;

        protected const float expected = 456F;
        protected const float successfulResult = Single.NaN;

        protected float SuccessfulRemoteOperation(IDummyProxyable someProxyable)
        {
            return someProxyable.PassInput(successfulResult);
        }

        protected float FailCallback(Exception ex, string path)
        {
            actualEx = ex;

            return expected;
        }

        public override void before_each()
        {
            base.before_each();

            expectedEx = new DummyTestException("This is supposed to fail");
        }

        [Test]
        public void it_should_return_failure_result()
        {
            actual.Should().Be(expected);
        }

        [Test]
        public void it_should_pass_exception_to_callback()
        {
            actualEx.Should().Be(expectedEx);
        }
    }

    public class CrossDomainRunner_when_creating_appdomain_fails : CrossDomainRunner_when_run_fails
    {
        public override void before_each()
        {
            base.before_each();

            appDomainFactory.Create(somePath).Returns(_ =>
            {
                throw expectedEx;
            });

            actual = runner.Run(somePath, SuccessfulRemoteOperation, FailCallback);
        }
    }

    public class CrossDomainRunner_when_creating_proxyable_fails : CrossDomainRunner_when_run_fails
    {
        public override void before_each()
        {
            base.before_each();

            proxyableFactory.CreateProxy(targetDomain).Returns(_ =>
            {
                throw expectedEx;
            });

            actual = runner.Run(somePath, SuccessfulRemoteOperation, FailCallback);
        }

        [Test]
        public void it_should_dispose_target_app_domain()
        {
            targetDomain.Received(1).Dispose();
        }
    }

    public class CrossDomainRunner_when_remote_operation_fails : CrossDomainRunner_when_run_fails
    {
        protected float FailingRemoteOperation(IDummyProxyable someProxyable)
        {
            throw expectedEx;
        }

        public override void before_each()
        {
            base.before_each();
            
            actual = runner.Run(somePath, FailingRemoteOperation, FailCallback);
        }

        [Test]
        public void it_should_dispose_target_app_domain()
        {
            targetDomain.Received(1).Dispose();
        }

        [Test]
        public void it_should_dispose_proxyable_executor()
        {
            proxyable.Received(1).Dispose();
        }
    }
}
