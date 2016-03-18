using AutofacContrib.NSubstitute;
using FluentAssertions;
using NSpec.Domain;
using NSpec.VsAdapter.Core.Execution;
using NSpec.VsAdapter.Core.Execution.Target;
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
    [Category("ExecutionReporter")]
    public class ExecutionReporter_desc_base
    {
        protected ExecutionReporter reporter;

        protected AutoSubstitute autoSubstitute;
        protected IProgressRecorder progressRecorder;
        protected IExecutedExampleMapper executedExampleMapper;

        [SetUp]
        public virtual void before_each()
        {
            autoSubstitute = new AutoSubstitute();

            progressRecorder = autoSubstitute.Resolve<IProgressRecorder>();

            executedExampleMapper = autoSubstitute.Resolve<IExecutedExampleMapper>();

            reporter = autoSubstitute.Resolve<ExecutionReporter>();
        }

        [TearDown]
        public virtual void after_each()
        {
            autoSubstitute.Dispose();
        }
    }

    public class ExecutionReporter_when_writing_example : ExecutionReporter_desc_base
    {
        ExecutedExample someExecutedExample;
        
        int someLevel = 123;

        public override void before_each()
        {
            base.before_each();

            var someContext = new Context("some context");

            Action someAction = () => { };

            var someExample = new Example("some-example-name", "some-tag another-tag", someAction, false)
            {
                Context = someContext,
                HasRun = true,
                Exception = new DummyTestException(),
            };

            someExecutedExample = new ExecutedExample()
            {
                FullName = someExample.FullName(),
                Failed = true,
                ExceptionMessage = someExample.Exception.Message,
                ExceptionStackTrace = someExample.Exception.StackTrace,
            };

            executedExampleMapper.FromExample(someExample).Returns(someExecutedExample);

            reporter.Write(someExample, someLevel);
        }

        [Test]
        public void it_should_record_result()
        {
            progressRecorder.Received(1).RecordExecutedExample(someExecutedExample);
        }
    }

    public class ExecutionReporter_when_writing_context : ExecutionReporter_desc_base
    {
        public override void before_each()
        {
            base.before_each();

            var someContext = new Context("some context");

            reporter.Write(someContext);
        }

        [Test]
        public void it_should_not_record_any_result()
        {
            progressRecorder.DidNotReceive().RecordExecutedExample(Arg.Any<ExecutedExample>());
        }
    }
}
