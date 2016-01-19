using AutofacContrib.NSubstitute;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using NSpec.Domain;
using NSpec.VsAdapter.Execution;
using NSpec.VsAdapter.TestAdapter;
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
    [Category("ExecutionObserver")]
    public abstract class ExecutionObserver_desc
    {
        protected ExecutionObserver observer;

        protected AutoSubstitute autoSubstitute;
        protected ITestExecutionRecorder testExecutionRecorder;

        [SetUp]
        public virtual void before_each()
        {
            autoSubstitute = new AutoSubstitute();

            testExecutionRecorder = autoSubstitute.Resolve<ITestExecutionRecorder>();

            observer = autoSubstitute.Resolve<ExecutionObserver>();
        }

        [TearDown]
        public virtual void after_each()
        {
            autoSubstitute.Dispose();
        }

        [Test]
        public void it_should_not_record_any_start()
        {
            testExecutionRecorder.DidNotReceive().RecordStart(Arg.Any<TestCase>());
        }

        [Test]
        public void it_should_not_record_any_end()
        {
            testExecutionRecorder.DidNotReceive().RecordEnd(Arg.Any<TestCase>(), Arg.Any<TestOutcome>());
        }

        [Test]
        public void it_should_not_record_any_attachment()
        {
            testExecutionRecorder.DidNotReceive().RecordAttachments(Arg.Any<IList<AttachmentSet>>());
        }
    }

    public class ExecutionObserver_when_writing_example : ExecutionObserver_desc
    {
        ITestResultMapper testResultMapper;

        TestResult someTestResult;
        const string somePath = @".\path\to\some\dummy-library.dll";
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

            var someTestCase = new TestCase(someExample.FullName(), new Uri("http://www.example.com"), somePath);

            someTestResult = new TestResult(someTestCase)
                {
                    Outcome = TestOutcome.Failed,
                };

            testResultMapper = autoSubstitute.Resolve<ITestResultMapper>();
            testResultMapper.FromExample(someExample, somePath).Returns(someTestResult);

            observer.BinaryPath = somePath;

            observer.Write(someExample, someLevel);
        }

        [Test]
        public void it_should_record_result()
        {
            testExecutionRecorder.Received(1).RecordResult(someTestResult);
        }
    }

    public class ExecutionObserver_when_writing_context : ExecutionObserver_desc
    {
        public override void before_each()
        {
            base.before_each();

            var someContext = new Context("some context");

            observer.Write(someContext);
        }

        [Test]
        public void it_should_not_record_any_result()
        {
            testExecutionRecorder.DidNotReceive().RecordResult(Arg.Any<TestResult>());
        }
    }
}
