using AutofacContrib.NSubstitute;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NSpec.Domain;
using NSpec.VsAdapter.Core.Discovery;
using NSpec.VsAdapter.Core.Discovery.Target;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.UnitTests.Core.Discovery.Target
{
    [TestFixture]
    [Category("DiscoveredExampleMapper")]
    public abstract class DiscoveredExampleMapper_desc_base
    {
        protected DiscoveredExampleMapper mapper;

        protected AutoSubstitute autoSubstitute;
        protected IDebugInfoProvider debugInfoProvider;

        protected readonly FixtureData fixtureData;
        protected readonly DiscoveredExample expected;

        // emulates private instance nspec.todo, defined in nspec ancestor
        protected readonly Action dummyTodo = () => { };

        protected const string someAssemblyPath = @".\some\path\to\assembly.dll";
        protected const string someSourceCodePath = @".\some\path\to\source\code.cs";
        protected const int someLineNumber = 123;

        public DiscoveredExampleMapper_desc_base()
        {
            var parentContext = new Context("some parent context");

            var context = new Context("some child context");
            context.Parent = parentContext;

            fixtureData = BuildFixtureData();

            var example = fixtureData.Example;
            example.Context = context;
            example.Spec = "some specification";

            expected = new DiscoveredExample()
            {
                FullName = example.FullName(),
                SourceAssembly = someAssemblyPath,
                SourceFilePath = someSourceCodePath,
                SourceLineNumber = someLineNumber,
                Tags = example.Tags.Select(tag => tag.Replace("_", " ")).ToArray(),
            };
        }

        [SetUp]
        public virtual void before_each()
        {
            autoSubstitute = new AutoSubstitute();

            debugInfoProvider = autoSubstitute.Resolve<IDebugInfoProvider>();

            string specClassName = this.GetType().ToString();
            string exampleMethodName = fixtureData.MethodName;
            var navigationData = new DiaNavigationData(someSourceCodePath, someLineNumber, someLineNumber + 4);
            var emptyNavigationData = new DiaNavigationData(String.Empty, 0, 0);

            debugInfoProvider.GetNavigationData(null, null).ReturnsForAnyArgs(emptyNavigationData);

            debugInfoProvider.GetNavigationData(
                Arg.Is<string>(arg => arg.StartsWith(specClassName, StringComparison.Ordinal)),
                exampleMethodName).Returns(navigationData);

            mapper = new DiscoveredExampleMapper(someAssemblyPath, debugInfoProvider);
        }

        [TearDown]
        public virtual void after_each()
        {
            autoSubstitute.Dispose();
        }

        protected abstract FixtureData BuildFixtureData();

        protected class FixtureData
        {
            public ExampleBase Example;
            public string MethodName;
        }
    }

    public class DiscoveredExampleMapper_when_example_is_runnable : DiscoveredExampleMapper_desc_base
    {
        protected override FixtureData BuildFixtureData()
        {
            Action someAction = () => { };

            ExampleBase someExample = new Example(
                "some-test-full-name",
                "tag1 tag2_more tag3",
                someAction);

            string someMethodName = someAction.Method.Name;

            return new FixtureData()
            {
                Example = someExample,
                MethodName = someMethodName,
            };
        }

        [Test]
        public void it_should_fill_all_details()
        {
            var actual = mapper.FromExample(fixtureData.Example);

            actual.ShouldBeEquivalentTo(expected);
        }
    }

    public class DiscoveredExampleMapper_when_example_is_pending : DiscoveredExampleMapper_desc_base
    {
        protected override FixtureData BuildFixtureData()
        {
            Action someAction = dummyTodo;

            ExampleBase someExample = new Example(
                "some-test-full-name",
                "tag1 tag2_more tag3",
                someAction,
                pending: true);

            string someMethodName = someAction.Method.Name;

            return new FixtureData()
            {
                Example = someExample,
                MethodName = someMethodName,
            };
        }

        [Test]
        public void it_should_lack_source_code_info()
        {
            var pendingExpected = new DiscoveredExample()
            {
                FullName = fixtureData.Example.FullName(),
                SourceAssembly = someAssemblyPath,
                SourceFilePath = String.Empty,
                SourceLineNumber = 0,
                Tags = fixtureData.Example.Tags.Select(tag => tag.Replace("_", " ")).ToArray(),
            };

            var actual = mapper.FromExample(fixtureData.Example);

            actual.ShouldBeEquivalentTo(pendingExpected);
        }
    }

    public class DiscoveredExampleMapper_when_example_is_method_runnable : DiscoveredExampleMapper_desc_base
    {
        public void DummyMethod() { }

        protected override FixtureData BuildFixtureData()
        {
            Action someAction = DummyMethod;

            MethodInfo methodInfo = someAction.Method;

            ExampleBase someExample = new MethodExample(
                methodInfo,
                "tag1 tag2_more tag3");

            string someMethodName = methodInfo.Name;

            return new FixtureData()
            {
                Example = someExample,
                MethodName = someMethodName,
            };
        }

        [Test]
        public void it_should_fill_all_details()
        {
            var actual = mapper.FromExample(fixtureData.Example);

            actual.ShouldBeEquivalentTo(expected);
        }
    }

    public class DiscoveredExampleMapper_when_example_is_async_runnable : DiscoveredExampleMapper_desc_base
    {
        protected override FixtureData BuildFixtureData()
        {
            Func<Task> someAsyncAction = async () => await Task.Run(() => { });

            ExampleBase someExample = new AsyncExample(
                "some-test-full-name",
                "tag1 tag2_more tag3",
                someAsyncAction);

            string someMethodName = someAsyncAction.Method.Name;

            return new FixtureData()
            {
                Example = someExample,
                MethodName = someMethodName,
            };
        }

        [Test]
        public void it_should_fill_all_details()
        {
            var actual = mapper.FromExample(fixtureData.Example);

            actual.ShouldBeEquivalentTo(expected);
        }
    }

    public class DiscoveredExampleMapper_when_example_is_async_method_runnable : DiscoveredExampleMapper_desc_base
    {
        public async Task DummyMethodAsync()
        {
            await Task.Run(() => { });
        }

        protected override FixtureData BuildFixtureData()
        {
            Func<Task> someAsyncAction = DummyMethodAsync;

            MethodInfo methodInfo = someAsyncAction.Method;

            ExampleBase someExample = new AsyncMethodExample(
                methodInfo,
                "tag1 tag2_more tag3");

            string someMethodName = someAsyncAction.Method.Name;

            return new FixtureData()
            {
                Example = someExample,
                MethodName = someMethodName,
            };
        }

        [Test]
        public void it_should_fill_all_details()
        {
            var actual = mapper.FromExample(fixtureData.Example);

            actual.ShouldBeEquivalentTo(expected);
        }
    }
}
