using AutofacContrib.NSubstitute;
using FluentAssertions;
using NSpec.VsAdapter.Execution;
using NSpec.VsAdapter.Logging;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
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
        protected ICrossDomainExecutor crossDomainExecutor;
        protected IExecutorInvocationFactory executorInvocationFactory;
        protected IExecutorInvocation executorInvocation;
        protected IProgressRecorder progressRecorder;
        protected IOutputLogger logger;
        protected IReplayLogger replayLogger;
        protected int actualCount;

        protected const string somePath = @".\path\to\some\dummy-library.dll";

        protected string[] testCaseFullNames = new string[]
        {
            "testCaseFullNames 1", "testCaseFullNames 2", "testCaseFullNames 3", "testCaseFullNames 4", 
        };

        [SetUp]
        public virtual void before_each()
        {
            autoSubstitute = new AutoSubstitute();

            crossDomainExecutor = autoSubstitute.Resolve<ICrossDomainExecutor>();
            executorInvocationFactory = autoSubstitute.Resolve<IExecutorInvocationFactory>();
            executorInvocation = autoSubstitute.Resolve<IExecutorInvocation>();

            progressRecorder = autoSubstitute.Resolve<IProgressRecorder>();
            logger = autoSubstitute.Resolve<IOutputLogger>();
            replayLogger = autoSubstitute.Resolve<IReplayLogger>();

            executor = autoSubstitute.Resolve<BinaryTestExecutor>();
        }

        [TearDown]
        public virtual void after_each()
        {
            autoSubstitute.Dispose();
        }
    }

    public abstract class BinaryTestExecutor_when_executing_by_source : BinaryTestExecutor_desc_base
    {
        public override void before_each()
        {
            base.before_each();

            executorInvocationFactory
                .Create(somePath, progressRecorder, Arg.Any<LogRecorder>())
                .Returns(executorInvocation);
        }

        [Test]
        public void it_should_request_invocation_by_source()
        {
            executorInvocationFactory.Received().Create(somePath, progressRecorder, Arg.Any<LogRecorder>());
        }
    }

    public class BinaryTestExecutor_when_executing_by_source_succeeds : BinaryTestExecutor_when_executing_by_source
    {
        const int expectedCount = 17;

        public override void before_each()
        {
            base.before_each();

            crossDomainExecutor.Run(somePath, executorInvocation.Execute).Returns(expectedCount);

            actualCount = executor.Execute(somePath, progressRecorder, logger, replayLogger);
        }

        [Test]
        public void it_should_return_nr_of_tests_ran()
        {
            actualCount.Should().Be(expectedCount);
        }
    }

    public class BinaryTestExecutor_when_execution_by_source_fails : BinaryTestExecutor_when_executing_by_source
    {
        public override void before_each()
        {
            base.before_each();

            crossDomainExecutor.Run(null, null).ReturnsForAnyArgs(_ =>
            {
                throw new DummyTestException();
            });

            actualCount = executor.Execute(somePath, progressRecorder, logger, replayLogger);
        }

        [Test]
        public void it_should_return_zero_tests_ran()
        {
            actualCount.Should().Be(0);
        }

        [Test]
        public void it_should_log_error_and_exception()
        {
            logger.Received(1).Error(Arg.Any<DummyTestException>(), Arg.Any<string>());
        }
    }

    public abstract class BinaryTestExecutor_when_executing_by_testcase : BinaryTestExecutor_desc_base
    {
        public override void before_each()
        {
            base.before_each();

            executorInvocationFactory
                .Create(somePath, Arg.Is<string[]>(names => names.SequenceEqual(testCaseFullNames)), progressRecorder, Arg.Any<LogRecorder>())
                .Returns(executorInvocation);
        }

        [Test]
        public void it_should_request_invocation_by_testcase()
        {
            executorInvocationFactory.Received().Create(somePath, Arg.Is<string[]>(names => names.SequenceEqual(testCaseFullNames)), progressRecorder, Arg.Any<LogRecorder>());
        }
    }

    public class BinaryTestExecutor_when_executing_by_testcase_succeeds : BinaryTestExecutor_when_executing_by_testcase
    {
        const int expectedCount = 17;

        public override void before_each()
        {
            base.before_each();

            crossDomainExecutor.Run(somePath, executorInvocation.Execute).Returns(expectedCount);

            actualCount = executor.Execute(somePath, testCaseFullNames, progressRecorder, logger, replayLogger);
        }

        [Test]
        public void it_should_return_nr_of_tests_ran()
        {
            actualCount.Should().Be(expectedCount);
        }
    }

    public class BinaryTestExecutor_when_executing_by_testcase_fails : BinaryTestExecutor_when_executing_by_testcase
    {
        public override void before_each()
        {
            base.before_each();

            crossDomainExecutor.Run(null, null).ReturnsForAnyArgs(_ =>
            {
                throw new DummyTestException();
            });

            actualCount = executor.Execute(somePath, testCaseFullNames, progressRecorder, logger, replayLogger);
        }

        [Test]
        public void it_should_return_zero_tests_ran()
        {
            actualCount.Should().Be(0);
        }

        [Test]
        public void it_should_log_error_and_exception()
        {
            logger.Received(1).Error(Arg.Any<DummyTestException>(), Arg.Any<string>());
        }
    }
}
