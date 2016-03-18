using AutofacContrib.NSubstitute;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using NSpec.Domain;
using NSpec.VsAdapter.Core.Execution;
using NSpec.VsAdapter.TestAdapter;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.UnitTests.Core.Execution
{
    [TestFixture]
    [Category("ProgressRecorder")]
    public abstract class ProgressRecorder_desc
    {
        protected ProgressRecorder recorder;

        protected AutoSubstitute autoSubstitute;
        protected ITestExecutionRecorder testExecutionRecorder;

        [SetUp]
        public virtual void before_each()
        {
            autoSubstitute = new AutoSubstitute();

            testExecutionRecorder = autoSubstitute.Resolve<ITestExecutionRecorder>();

            recorder = autoSubstitute.Resolve<ProgressRecorder>();
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

        [Test]
        public void it_should_claim_infinite_lease_lifetime()
        {
            recorder.GetLifetimeService().Should().BeNull();
        }
    }

    public class ProgressRecorder_when_recording_executed_example : ProgressRecorder_desc
    {
        TestResult someTestResult;
        
        const string somePath = @".\path\to\some\dummy-library.dll";

        public override void before_each()
        {
            base.before_each();

            var someExample = new ExecutedExample()
            {
                FullName = "nspec. some context. some passing example.",
                Pending = false,
                Failed = false,
            };

            var someTestCase = new TestCase(someExample.FullName, new Uri("http://www.example.com"), somePath);

            someTestResult = new TestResult(someTestCase)
                {
                    Outcome = TestOutcome.Failed,
                };

            var testResultMapper = autoSubstitute.Resolve<ITestResultMapper>();
            testResultMapper.FromExecutedExample(someExample, somePath).Returns(someTestResult);

            recorder.BinaryPath = somePath;

            recorder.RecordExecutedExample(someExample);
        }

        [Test]
        public void it_should_record_result()
        {
            testExecutionRecorder.Received(1).RecordResult(someTestResult);
        }
    }
}
