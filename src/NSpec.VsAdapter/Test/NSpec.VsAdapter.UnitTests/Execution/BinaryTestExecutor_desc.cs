using AutofacContrib.NSubstitute;
using FluentAssertions;
using NSpec.VsAdapter.Execution;
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
    public abstract class CrossDomainTestExecutor_desc_base
    {
        protected BinaryTestExecutor executor;

        protected AutoSubstitute autoSubstitute;
        protected ICrossDomainExecutor crossDomainExecutor;
        protected IExecutorInvocationFactory executorInvocationFactory;
        protected IExecutorInvocation executorInvocation;
        protected IExecutionObserver executionObserver;
        protected IOutputLogger logger;
        protected IReplayLogger crossDomainLogger;

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

            executionObserver = autoSubstitute.Resolve<IExecutionObserver>();
            logger = autoSubstitute.Resolve<IOutputLogger>();
            crossDomainLogger = autoSubstitute.Resolve<IReplayLogger>();

            executor = autoSubstitute.Resolve<BinaryTestExecutor>();
        }

        [TearDown]
        public virtual void after_each()
        {
            autoSubstitute.Dispose();
        }
    }

    public abstract class CrossDomainTestExecutor_when_executing_by_source : CrossDomainTestExecutor_desc_base
    {
        public override void before_each()
        {
            base.before_each();

            executorInvocationFactory
                .Create(somePath, executionObserver, Arg.Any<LogRecorder>())
                .Returns(executorInvocation);
        }

        [Test]
        public void it_should_request_invocation_by_source()
        {
            executorInvocationFactory.Received().Create(somePath, executionObserver, Arg.Any<LogRecorder>());
        }
    }

    public class CrossDomainTestExecutor_when_executing_by_source_succeeds : CrossDomainTestExecutor_when_executing_by_source
    {
        public override void before_each()
        {
            base.before_each();

            executor.Execute(somePath, executionObserver, logger, crossDomainLogger);
        }

        [Test]
        public void it_should_run_operator_on_source()
        {
            crossDomainExecutor.Received().Run(somePath, executorInvocation.Operate);
        }
    }

    public class CrossDomainTestExecutor_when_execution_by_source_fails : CrossDomainTestExecutor_when_executing_by_source
    {
        public override void before_each()
        {
            base.before_each();

            crossDomainExecutor.Run(null, null).ReturnsForAnyArgs(_ =>
            {
                throw new DummyTestException();
            });

            executor.Execute(somePath, executionObserver, logger, crossDomainLogger);
        }

        [Test]
        public void it_should_log_error_and_exception()
        {
            logger.Received(1).Error(Arg.Any<DummyTestException>(), Arg.Any<string>());
        }
    }

    public abstract class CrossDomainTestExecutor_when_executing_by_testcase : CrossDomainTestExecutor_desc_base
    {
        public override void before_each()
        {
            base.before_each();

            executorInvocationFactory
                .Create(somePath, testCaseFullNames, executionObserver, Arg.Any<LogRecorder>())
                .Returns(executorInvocation);
        }

        [Test]
        public void it_should_request_invocation_by_testcase()
        {
            var argMatcher = Arg.Is<string[]>(names => MatchEnumerables(names, testCaseFullNames));

            executorInvocationFactory.Received().Create(somePath, argMatcher, executionObserver, Arg.Any<LogRecorder>());
        }

        static bool MatchEnumerables(IEnumerable<string> a, IEnumerable<string> b)
        {
            try
            {
                a.ShouldBeEquivalentTo(b);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

    public class CrossDomainTestExecutor_when_executing_by_testcase_succeeds : CrossDomainTestExecutor_when_executing_by_testcase
    {
        public override void before_each()
        {
            base.before_each();

            executor.Execute(somePath, testCaseFullNames, executionObserver, logger, crossDomainLogger);
        }

        [Test]
        [Ignore("It fails on ExecutorInvocation.Operate")]
        public void it_should_run_operator_on_source()
        {
            crossDomainExecutor.Received().Run(somePath, executorInvocation.Operate);
        }
    }

    public class CrossDomainTestExecutor_when_executing_by_testcase_fails : CrossDomainTestExecutor_when_executing_by_testcase
    {
        public override void before_each()
        {
            base.before_each();

            crossDomainExecutor.Run(null, null).ReturnsForAnyArgs(_ =>
            {
                throw new DummyTestException();
            });

            executor.Execute(somePath, testCaseFullNames, executionObserver, logger, crossDomainLogger);
        }

        [Test]
        public void it_should_log_error_and_exception()
        {
            logger.Received(1).Error(Arg.Any<DummyTestException>(), Arg.Any<string>());
        }
    }
}
