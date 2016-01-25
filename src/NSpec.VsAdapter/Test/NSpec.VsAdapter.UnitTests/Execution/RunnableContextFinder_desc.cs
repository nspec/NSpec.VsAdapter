using AutofacContrib.NSubstitute;
using FluentAssertions;
using NSpec.Domain;
using NSpec.VsAdapter.Common;
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
    [Category("RunnableContextFinder")]
    public abstract class RunnableContextFinder_desc_base
    {
        protected RunnableContextFinder runnableContextFinder;

        protected AutoSubstitute autoSubstitute;
        protected IContextFinder contextFinder;

        protected const string somePath = @".\path\to\some\dummy-library.dll";
        protected readonly Context[] someContexts;
        protected readonly ContextCollection someContextCollection;

        public RunnableContextFinder_desc_base()
        {
            var context1 = new Context("context 1");

            context1.AddExample(new Example("example 1A"));
            context1.AddExample(new Example("example 1B"));

            var context2 = new Context("context 2");

            context2.AddExample(new Example("example 2A"));

            var context3 = new Context("context 3");

            context3.AddExample(new Example("example 3A"));
            context3.AddExample(new Example("example 3B"));
            context3.AddExample(new Example("example 3C"));

            var context4 = new Context("context 4");

            context4.AddExample(new Example("example 4A"));

            someContexts = new Context[]
            {
                context1,
                context2,
                context3,
                context4,
            };

            someContextCollection = new ContextCollection(someContexts);
        }

        [SetUp]
        public virtual void before_each()
        {
            autoSubstitute = new AutoSubstitute();

            contextFinder = autoSubstitute.Resolve<IContextFinder>();

            runnableContextFinder = autoSubstitute.Resolve<RunnableContextFinder>();

            contextFinder.BuildContextCollection(somePath).Returns(someContextCollection);
        }

        [TearDown]
        public virtual void after_each()
        {
            autoSubstitute.Dispose();
        }
    }

    public class RunnableContextFinder_when_finding_all : RunnableContextFinder_desc_base
    {
        [Test]
        public void it_should_return_all_contexts()
        {
            var actualContexts = runnableContextFinder.Find(somePath, RunnableContextFinder.RunAll);

            actualContexts.ShouldBeEquivalentTo(someContexts);
        }
    }

    public class RunnableContextFinder_when_finding_example_names : RunnableContextFinder_desc_base
    {
        [Test]
        public void it_should_return_only_containing_contexts()
        {
            var exampleNames = new string[]
            {
                "context 1. example 1B.",
                "context 3. example 3A.",
                "context 3. example 3C.",
            };

            var expectedContexts = new Context[]
            {
                someContexts[0],
                someContexts[2],
            };

            var actualContexts = runnableContextFinder.Find(somePath, exampleNames);

            actualContexts.ShouldBeEquivalentTo(expectedContexts);
        }
    }

    public class RunnableContextFinder_when_finding_example_names_and_no_first_level_examples : RunnableContextFinder_desc_base
    {
        public override void before_each()
        {
            base.before_each();

            var firstLevelContext = new Context("first-level context");

            foreach (var ctx in someContexts)
            {
                ctx.Parent = firstLevelContext;
            }

            firstLevelContext.Contexts = someContextCollection;

            var firstLevelContextCollection = new ContextCollection(new[] { firstLevelContext });

            contextFinder.BuildContextCollection(somePath).Returns(firstLevelContextCollection);
        }

        [Test]
        public void it_should_return_only_containing_contexts()
        {
            var exampleNames = new string[]
            {
                "first-level context. context 1. example 1B.",
                "first-level context. context 3. example 3A.",
                "first-level context. context 3. example 3C.",
            };

            var expectedContexts = new Context[]
            {
                someContexts[0],
                someContexts[2],
            };

            var actualContexts = runnableContextFinder.Find(somePath, exampleNames);

            actualContexts.ShouldBeEquivalentTo(expectedContexts);
        }
    }
}
