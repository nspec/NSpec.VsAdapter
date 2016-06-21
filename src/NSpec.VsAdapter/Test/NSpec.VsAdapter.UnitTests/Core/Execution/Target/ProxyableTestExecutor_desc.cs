using AutofacContrib.NSubstitute;
using FluentAssertions;
using NSpec.Domain;
using NSpec.Domain.Formatters;
using NSpec.VsAdapter.Core.Execution;
using NSpec.VsAdapter.Core.Execution.Target;
using NSpec.VsAdapter.Logging;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.UnitTests.Core.Execution.Target
{
    [TestFixture]
    [Category("ProxyableTestExecutor")]
    public abstract class ProxyableTestExecutor_desc_base
    {
        protected ProxyableTestExecutor executor;

        protected AutoSubstitute autoSubstitute;
        protected IRunnableContextFinder runnableContextFinder;
        protected IExecutionReporterFactory executionReporterFactory;
        protected IContextExecutorFactory contextExecutorFactory;
        protected IProgressRecorder progressRecorder;
        protected ICrossDomainLogger crossDomainLogger;
        protected ILiveFormatter executionReporter;

        protected int actual;

        protected readonly string[] someExampleFullNames =
        {
            "source-1-context-1-spec-A", "source-1-context-1-spec-B", "source-1-context-2-spec-C",
        };
        protected readonly IEnumerable<RunnableContext> someRunnableContexts;

        protected const string somePath = @".\path\to\some\dummy-library.dll";

        public ProxyableTestExecutor_desc_base()
        {
            Context[] someContexts =
            {
                new Context("context-1"),
                new Context("context-2"),
            };

            someRunnableContexts = someContexts.Select(ctx => new RunnableContext(ctx));
        }

        [SetUp]
        public virtual void before_each()
        {
            autoSubstitute = new AutoSubstitute();

            runnableContextFinder = autoSubstitute.Resolve<IRunnableContextFinder>();
            executionReporterFactory = autoSubstitute.Resolve<IExecutionReporterFactory>();
            contextExecutorFactory = autoSubstitute.Resolve<IContextExecutorFactory>();

            progressRecorder = Substitute.For<IProgressRecorder>();
            crossDomainLogger = Substitute.For<ICrossDomainLogger>();
            executionReporter = Substitute.For<ILiveFormatter>();

            runnableContextFinder.Find(somePath, someExampleFullNames).Returns(someRunnableContexts);
            executionReporterFactory.Create(progressRecorder).Returns(executionReporter);

            executor = autoSubstitute.Resolve<ProxyableTestExecutor>();
        }

        [TearDown]
        public virtual void after_each()
        {
            autoSubstitute.Dispose();
            executor.Dispose();
        }

        protected abstract void act_each();
    }

    public abstract class ProxyableTestExecutor_when_execute_succeeds : ProxyableTestExecutor_desc_base
    {
        public override void before_each()
        {
            base.before_each();

            act_each();
        }

        [Test]
        public void it_should_return_executed_example_count()
        {
            int expected = -1;

            actual.Should().Be(expected);
        }
    }

    public class ProxyableTestExecutor_when_execute_all_succeeds : ProxyableTestExecutor_when_execute_succeeds
    {
        protected override void act_each()
        {
            actual = executor.ExecuteAll(somePath, progressRecorder, crossDomainLogger);
        }
    }

    public class ProxyableTestExecutor_when_execute_some_succeeds : ProxyableTestExecutor_when_execute_succeeds
    {
        protected override void act_each()
        {
            actual = executor.ExecuteSelection(somePath, someExampleFullNames, progressRecorder, crossDomainLogger);
        }
    }

    public abstract class ProxyableTestExecutor_when_execute_fails : ProxyableTestExecutor_desc_base
    {
        protected DummyTestException expectedEx;

        public override void before_each()
        {
            base.before_each();

            expectedEx = new DummyTestException();
        }

        protected override void act_each()
        {
            // Skip doubling all failing tests for ExecuteSelection scenario, just test ExecuteAll scenario

            actual = executor.ExecuteAll(somePath, progressRecorder, crossDomainLogger);
        }

        [Test]
        public void it_should_return_zero_executed_examples()
        {
            int expected = -1;

            actual.Should().Be(expected);
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

    public class ProxyableTestExecutor_when_finding_contexts_fails : ProxyableTestExecutor_when_execute_fails
    {
    }

    public class ProxyableTestExecutor_when_executing_contexts_fails : ProxyableTestExecutor_when_execute_fails
    {
    }
}
