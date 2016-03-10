using AutofacContrib.NSubstitute;
using FluentAssertions;
using NSpec.VsAdapter.CrossDomain;
using NSpec.VsAdapter.Execution;
using NSpec.VsAdapter.Logging;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.UnitTests.Execution
{
    [TestFixture]
    [Category("BinaryTestExecutor")]
    public abstract class BinaryTestExecutor_desc_base
    {
        protected BinaryTestExecutor executor;

        protected AutoSubstitute autoSubstitute;
        protected IAppDomainFactory appDomainFactory;
        protected ITargetAppDomain targetDomain;
        protected IProxyableFactory<IProxyableTestExecutor> proxyableFactory;
        protected IProxyableTestExecutor proxyableExecutor;
        protected IProgressRecorder progressRecorder;
        protected IOutputLogger logger;
        protected ICrossDomainLogger crossDomainLogger;
        protected int actualCount;

        protected const string somePath = @".\path\to\some\dummy-library.dll";

        protected string[] testCaseFullNames = new string[]
        {
            "testCaseFullNames 1", "testCaseFullNames 2", "testCaseFullNames 3", "testCaseFullNames 4", 
        };

        public BinaryTestExecutor_desc_base()
        {
            CompareToTestCaseFullNames = 
                inputNames => testCaseFullNames.SequenceEqual(inputNames);
        }

        [SetUp]
        public virtual void before_each()
        {
            autoSubstitute = new AutoSubstitute();

            targetDomain = Substitute.For<ITargetAppDomain>();
            appDomainFactory = autoSubstitute.Resolve<IAppDomainFactory>();
            appDomainFactory.Create(somePath).Returns(targetDomain);

            proxyableExecutor = Substitute.For<IProxyableTestExecutor>();
            proxyableFactory = autoSubstitute.Resolve<IProxyableFactory<IProxyableTestExecutor>>();
            proxyableFactory.CreateProxy(targetDomain).Returns(proxyableExecutor);

            progressRecorder = autoSubstitute.Resolve<IProgressRecorder>();
            logger = autoSubstitute.Resolve<IOutputLogger>();
            crossDomainLogger = autoSubstitute.Resolve<ICrossDomainLogger>();

            executor = autoSubstitute.Resolve<BinaryTestExecutor>();
        }

        [TearDown]
        public virtual void after_each()
        {
            autoSubstitute.Dispose();
        }

        protected Expression<Predicate<string[]>> CompareToTestCaseFullNames;
    }

    public abstract class BinaryTestExecutor_when_executing_succeeds : BinaryTestExecutor_desc_base
    {
        protected const int expectedCount = 17;

        [Test]
        public void it_should_return_nr_of_tests_ran()
        {
            actualCount.Should().Be(expectedCount);
        }

        [Test]
        public void it_should_dispose_target_app_domain()
        {
            targetDomain.Received(1).Dispose();
        }

        [Test]
        public void it_should_dispose_proxyable_executor()
        {
            proxyableExecutor.Received(1).Dispose();
        }
    }

    public class BinaryTestExecutor_when_executing_all_succeeds : BinaryTestExecutor_when_executing_succeeds
    {
        public override void before_each()
        {
            base.before_each();

            proxyableExecutor.ExecuteAll(somePath, progressRecorder, crossDomainLogger).Returns(expectedCount);

            actualCount = executor.ExecuteAll(somePath, progressRecorder, logger, crossDomainLogger);
        }
    }

    public class BinaryTestExecutor_when_executing_a_selection_succeeds : BinaryTestExecutor_when_executing_succeeds
    {
        public override void before_each()
        {
            base.before_each();

            proxyableExecutor.ExecuteSelection(somePath,
                Arg.Is<string[]>(CompareToTestCaseFullNames),
                progressRecorder, crossDomainLogger).Returns(expectedCount);

            actualCount = executor.ExecuteSelected(somePath, testCaseFullNames, progressRecorder, logger, crossDomainLogger);
        }
    }

    public abstract class BinaryTestExecutor_when_executing_fails : BinaryTestExecutor_desc_base
    {
        protected DummyTestException ex;

        [Test]
        public void it_should_return_zero_tests_ran()
        {
            actualCount.Should().Be(0);
        }

        [Test]
        public void it_should_log_error_and_exception()
        {
            logger.Received(1).Error(ex, Arg.Any<string>());
        }
    }

    public class BinaryTestExecutor_when_creating_appdomain_fails : BinaryTestExecutor_when_executing_fails
    {
        public override void before_each()
        {
            base.before_each();

            appDomainFactory.Create(somePath).Returns(_ =>
            {
                ex = new DummyTestException();
                throw ex;
            });

            actualCount = executor.ExecuteAll(somePath, progressRecorder, logger, crossDomainLogger);
        }
    }

    public class BinaryTestExecutor_when_creating_proxyable_fails : BinaryTestExecutor_when_executing_fails
    {
        public override void before_each()
        {
            base.before_each();

            proxyableFactory.CreateProxy(targetDomain).Returns(_ =>
            {
                ex = new DummyTestException();
                throw ex;
            });

            actualCount = executor.ExecuteAll(somePath, progressRecorder, logger, crossDomainLogger);
        }

        [Test]
        public void it_should_dispose_target_app_domain()
        {
            targetDomain.Received(1).Dispose();
        }
    }

    public class BinaryTestExecutor_when_executing_all_fails : BinaryTestExecutor_when_executing_fails
    {
        public override void before_each()
        {
            base.before_each();

            proxyableExecutor.ExecuteAll(somePath, progressRecorder, crossDomainLogger).Returns(_ =>
            {
                ex = new DummyTestException();
                throw ex;
            });

            actualCount = executor.ExecuteAll(somePath, progressRecorder, logger, crossDomainLogger);
        }

        [Test]
        public void it_should_dispose_target_app_domain()
        {
            targetDomain.Received(1).Dispose();
        }

        [Test]
        public void it_should_dispose_proxyable_executor()
        {
            proxyableExecutor.Received(1).Dispose();
        }
    }

    public class BinaryTestExecutor_when_executing_a_selection_fails : BinaryTestExecutor_when_executing_fails
    {
        public override void before_each()
        {
            base.before_each();

            proxyableExecutor.ExecuteSelection(somePath,
                Arg.Is<string[]>(CompareToTestCaseFullNames), 
                progressRecorder, crossDomainLogger).Returns(_ =>
            {
                ex = new DummyTestException();
                throw ex;
            });

            actualCount = executor.ExecuteSelected(somePath, testCaseFullNames, progressRecorder, logger, crossDomainLogger);
        }

        [Test]
        public void it_should_dispose_target_app_domain()
        {
            targetDomain.Received(1).Dispose();
        }

        [Test]
        public void it_should_dispose_proxyable_executor()
        {
            proxyableExecutor.Received(1).Dispose();
        }
    }
}
