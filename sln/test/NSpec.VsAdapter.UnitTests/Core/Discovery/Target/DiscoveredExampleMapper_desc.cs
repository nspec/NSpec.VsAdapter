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

        protected ExampleBase example;
        protected string specClassName;
        protected string exampleMethodName;
        protected DiscoveredExample actual;
        protected DiscoveredExample expected;

        protected readonly Context context;
        protected readonly DiaNavigationData navigationData = new DiaNavigationData(someSourceCodePath, someLineNumber, someLineNumber + 4);

        // emulates private instance nspec.todo, defined in nspec ancestor
        protected readonly Action dummyTodo = () => { };

        protected const string someAssemblyPath = @".\some\path\to\assembly.dll";
        protected const string someSourceCodePath = @".\some\path\to\source\code.cs";
        protected const int someLineNumber = 123;

        public DiscoveredExampleMapper_desc_base()
        {
            var parentContext = new Context("some parent context");

            context = new Context("some child context");
            context.Parent = parentContext;
        }

        void InitTestData()
        {
            var fixtureData = BuildFixtureData();

            example = fixtureData.Instance;
            example.Context = context;
            example.Spec = "some specification";

            specClassName = fixtureData.ClassName;

            exampleMethodName = fixtureData.MethodName;

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

            InitTestData();

            debugInfoProvider = autoSubstitute.Resolve<IDebugInfoProvider>();

            var emptyNavigationData = new DiaNavigationData(String.Empty, 0, 0);

            // always return empty navigation data, unless specific inputs are provided

            debugInfoProvider.GetNavigationData(null, null).ReturnsForAnyArgs(emptyNavigationData);

            debugInfoProvider.GetNavigationData(specClassName, exampleMethodName).Returns(navigationData);

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
            public ExampleBase Instance;
            public string ClassName;
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

            string someExampleMethodName = someAction.Method.Name;

            return new FixtureData()
            {
                Instance = someExample,
                ClassName = someAction.Target.ToString(),
                MethodName = someExampleMethodName,
            };
        }

        [Test]
        public void it_should_fill_all_details()
        {
            actual = mapper.FromExample(example);

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

            string someExampleMethodName = someAction.Method.Name;

            return new FixtureData()
            {
                Instance = someExample,
                ClassName = String.Empty,
                MethodName = String.Empty,
            };
        }

        [Test]
        public void it_should_lack_source_code_info()
        {
            var pendingExpected = new DiscoveredExample()
            {
                FullName = example.FullName(),
                SourceAssembly = someAssemblyPath,
                SourceFilePath = String.Empty,
                SourceLineNumber = 0,
                Tags = example.Tags.Select(tag => tag.Replace("_", " ")).ToArray(),
            };

            actual = mapper.FromExample(example);

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

            string someExampleMethodName = methodInfo.Name;

            return new FixtureData()
            {
                Instance = someExample,
                ClassName = someAction.Target.ToString(),
                MethodName = someExampleMethodName,
            };
        }

        [Test]
        public void it_should_fill_all_details()
        {
            actual = mapper.FromExample(example);

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

            string someExampleMethodName = someAsyncAction.Method.Name;

            return new FixtureData()
            {
                Instance = someExample,
                ClassName = someAsyncAction.Target.ToString(),
                MethodName = someExampleMethodName,
            };
        }

        [Test]
        public void it_should_fill_all_details()
        {
            actual = mapper.FromExample(example);

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

            string someExampleMethodName = someAsyncAction.Method.Name;

            return new FixtureData()
            {
                Instance = someExample,
                ClassName = someAsyncAction.Target.ToString(),
                MethodName = someExampleMethodName,
            };
        }

        [Test]
        public void it_should_fill_all_details()
        {
            actual = mapper.FromExample(example);

            actual.ShouldBeEquivalentTo(expected);
        }
    }
}
