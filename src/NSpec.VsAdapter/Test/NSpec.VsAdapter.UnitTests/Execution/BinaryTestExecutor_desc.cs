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
        protected ICrossDomainRunner<IProxyableTestExecutor, int> remoteRunner;
        protected IProxyableTestExecutor proxyableExecutor;
        protected IProgressRecorder progressRecorder;
        protected IOutputLogger logger;
        protected ICrossDomainLogger crossDomainLogger;
        protected int actual;

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

            proxyableExecutor = Substitute.For<IProxyableTestExecutor>();

            remoteRunner = autoSubstitute.SubstituteFor<ICrossDomainRunner<IProxyableTestExecutor, int>>();

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
        protected const int expected = 17;

        public override void before_each()
        {
            base.before_each();

            remoteRunner
                .Run(somePath,
                    Arg.Any<Func<IProxyableTestExecutor, int>>(),
                    Arg.Any<Func<Exception, string, int>>())
                .Returns(callInfo =>
                {
                    var path = callInfo.Arg<string>();
                    var operation = callInfo.Arg<Func<IProxyableTestExecutor, int>>();

                    return operation(proxyableExecutor);
                });
        }

        [Test]
        public void it_should_return_nr_of_tests_ran()
        {
            actual.Should().Be(expected);
        }
    }

    public class BinaryTestExecutor_when_executing_all_succeeds : BinaryTestExecutor_when_executing_succeeds
    {
        public override void before_each()
        {
            base.before_each();

            proxyableExecutor.ExecuteAll(somePath, progressRecorder, crossDomainLogger).Returns(expected);

            actual = executor.ExecuteAll(somePath, progressRecorder, logger, crossDomainLogger);
        }
    }

    public class BinaryTestExecutor_when_executing_a_selection_succeeds : BinaryTestExecutor_when_executing_succeeds
    {
        public override void before_each()
        {
            base.before_each();

            proxyableExecutor.ExecuteSelection(somePath, Arg.Is<string[]>(CompareToTestCaseFullNames),
                progressRecorder, crossDomainLogger).Returns(expected);

            actual = executor.ExecuteSelected(somePath, testCaseFullNames, progressRecorder, logger, crossDomainLogger);
        }
    }

    public abstract class BinaryTestExecutor_when_executing_fails : BinaryTestExecutor_desc_base
    {
        protected DummyTestException expectedEx;

        public override void before_each()
        {
            base.before_each();

            expectedEx = new DummyTestException();

            remoteRunner
                .Run(somePath,
                    Arg.Any<Func<IProxyableTestExecutor, int>>(),
                    Arg.Any<Func<Exception, string, int>>())
                .Returns(callInfo =>
                {
                    var path = callInfo.Arg<string>();
                    var fail = callInfo.Arg<Func<Exception, string, int>>();

                    return fail(expectedEx, path);
                });
        }

        [Test]
        public void it_should_return_zero_tests_ran()
        {
            actual.Should().Be(0);
        }

        [Test]
        public void it_should_log_error_with_exception()
        {
            logger.Received(1).Error(expectedEx, Arg.Any<string>());
        }
    }

    public class BinaryTestExecutor_when_executing_all_locally_fails : BinaryTestExecutor_when_executing_fails
    {
        public override void before_each()
        {
            base.before_each();

            actual = executor.ExecuteAll(somePath, progressRecorder, logger, crossDomainLogger);
        }
    }

    public class BinaryTestExecutor_when_executing_a_selection_locally_fails : BinaryTestExecutor_when_executing_fails
    {
        public override void before_each()
        {
            base.before_each();

            actual = executor.ExecuteSelected(somePath, testCaseFullNames, progressRecorder, logger, crossDomainLogger);
        }
    }
}
