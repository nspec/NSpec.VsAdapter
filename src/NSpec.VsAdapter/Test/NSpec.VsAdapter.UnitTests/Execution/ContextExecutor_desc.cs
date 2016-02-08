using AutofacContrib.NSubstitute;
using FluentAssertions;
using NSpec.Domain.Formatters;
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
    [Category("ContextExecutor")]
    public abstract class ContextExecutor_desc_base
    {
        protected ContextExecutor executor;

        protected AutoSubstitute autoSubstitute;
        protected ILiveFormatter reporter;
        protected IExecutionCanceler canceler;

        [SetUp]
        public virtual void before_each()
        {
            autoSubstitute = new AutoSubstitute();

            reporter = autoSubstitute.Resolve<ILiveFormatter>();
            canceler = autoSubstitute.Resolve<IExecutionCanceler>();

            executor = autoSubstitute.Resolve<ContextExecutor>();
        }

        [TearDown]
        public virtual void after_each()
        {
            autoSubstitute.Dispose();
        }
    }

    public class ContextExecutor_when_executing : ContextExecutor_desc_base
    {
        readonly IRunnableContext[] runnableContexts;

        public ContextExecutor_when_executing()
        {
            runnableContexts = new IRunnableContext[]
            {
                Substitute.For<IRunnableContext>(),
                Substitute.For<IRunnableContext>(),
                Substitute.For<IRunnableContext>(),
            };
        }

        public override void before_each()
        {
            base.before_each();

            canceler.IsCanceled.Returns(false);
        }

        [Test]
        public void it_should_return_example_count()
        {
            var randomGen = new Random();
            int expected = 0;

            foreach (var context in runnableContexts)
            {
                int count = randomGen.Next(20);

                context.ExampleCount.Returns(count);

                expected += count;
            }

            int actual = executor.Execute(runnableContexts);

            actual.Should().Be(expected);
        }

        [Test]
        public void it_should_pass_reporter_to_all()
        {
            executor.Execute(runnableContexts);

            foreach (var context in runnableContexts)
            {
                context.Received(1).Run(reporter);
            }
        }
    }

    public class ContextExecutor_when_canceling : ContextExecutor_desc_base
    {
        [Test]
        [Ignore("Cannot figure out how to block & sync to inner Execute loop")]
        public void it_should_stop_execution()
        {
        }
    }
}
