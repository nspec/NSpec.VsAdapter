using AutofacContrib.NSubstitute;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NSpec.Domain;
using NSpec.VsAdapter.Discovery;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.UnitTests.Discovery
{
    [TestFixture]
    [Category("SpecMapper")]
    public abstract class SpecMapper_desc_base
    {
        protected SpecMapper mapper;

        protected AutoSubstitute autoSubstitute;
        protected IDebugInfoProvider debugInfoProvider;
        protected Context context;

        // emulates private instance nspec.todo, defined in nspec ancestor
        protected readonly Action dummyTodo = () => { };

        protected const string someAssemblyPath = @".\some\path\to\assembly.dll";
        protected const string someSourceCodePath = @".\some\path\to\source\code.cs";
        protected const int someLineNumber = 123;

        [SetUp]
        public virtual void before_each()
        {
            autoSubstitute = new AutoSubstitute();

            var parentContext = new Context("some parent context");

            context = new Context("some child context");
            context.Parent = parentContext;

            debugInfoProvider = autoSubstitute.Resolve<IDebugInfoProvider>();

            var emptyNavigationData = new DiaNavigationData(String.Empty, 0, 0);
            debugInfoProvider.GetNavigationData(null, null).ReturnsForAnyArgs(emptyNavigationData);

            mapper = new SpecMapper(someAssemblyPath, debugInfoProvider);
        }

        [TearDown]
        public virtual void after_each()
        {
            autoSubstitute.Dispose();
        }
    }

    public class SpecMapper_when_example_runs : SpecMapper_desc_base
    {
        Example example;

        public override void before_each()
        {
            base.before_each();

            Action someAction = () => { };

            example = new Example(
                "some-test-full-name",
                "tag1 tag2_more tag3",
                someAction);

            string specClassName = this.GetType().ToString();
            string exampleMethodName = someAction.Method.Name;

            example.Context = context;

            example.Spec = "some specification";

            var navigationData = new DiaNavigationData(someSourceCodePath, someLineNumber, someLineNumber + 4);

            debugInfoProvider.GetNavigationData(specClassName, exampleMethodName).Returns(navigationData);
        }

        [Test]
        public void it_should_fill_all_details()
        {
            var expected = new DiscoveredExample()
            {
                FullName = example.FullName(),
                SourceAssembly = someAssemblyPath,
                SourceFilePath = someSourceCodePath,
                SourceLineNumber = someLineNumber,
                Tags = example.Tags.Select(tag => tag.Replace("_", " ")).ToArray(),
            };

            var actual = mapper.FromExample(example);

            actual.ShouldBeEquivalentTo(expected);
        }
    }

    public class SpecMapper_when_example_is_pending : SpecMapper_desc_base
    {
        Example example;

        public override void before_each()
        {
            base.before_each();

            example = new Example(
                "some-test-full-name",
                "tag1 tag2_more tag3",
                dummyTodo,
                pending: true);

            string specClassName = this.GetType().ToString();
            string exampleMethodName = dummyTodo.Method.Name;

            example.Context = context;

            example.Spec = "some specification";

            var navigationData = new DiaNavigationData(someSourceCodePath, someLineNumber, someLineNumber + 4);

            debugInfoProvider.GetNavigationData(specClassName, exampleMethodName).Returns(navigationData);
        }

        [Test]
        public void it_should_lack_source_code_info()
        {
            var expected = new DiscoveredExample()
            {
                FullName = example.FullName(),
                SourceAssembly = someAssemblyPath,
                SourceFilePath = String.Empty,
                SourceLineNumber = 0,
                Tags = example.Tags.Select(tag => tag.Replace("_", " ")).ToArray(),
            };

            var actual = mapper.FromExample(example);

            actual.ShouldBeEquivalentTo(expected);
        }
    }
}
