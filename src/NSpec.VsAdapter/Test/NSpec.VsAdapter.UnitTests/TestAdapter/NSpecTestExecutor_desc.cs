using AutofacContrib.NSubstitute;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using NSpec.VsAdapter.Execution;
using NSpec.VsAdapter.TestAdapter;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.UnitTests.TestAdapter
{
    [TestFixture]
    [Category("NSpecTestExecutor")]
    public abstract class NSpecTestExecutor_desc_base
    {
        protected NSpecTestExecutor executor;

        protected AutoSubstitute autoSubstitute;
        protected ICrossDomainTestExecutor crossDomainTestExecutor;
        protected IExecutionObserver executionObserver;
        protected OutputLogger outputLogger;
        protected IRunContext runContext;
        protected IFrameworkHandle frameworkHandle;
        protected string[] sources;

        public NSpecTestExecutor_desc_base()
        {
            sources = new string[]
            {
                @".\path\to\some\dummy-library.dll",
                @".\other\path\to\another-dummy-library.dll",
            };
        }

        [SetUp]
        public virtual void before_each()
        {
            autoSubstitute = new AutoSubstitute();

            runContext = autoSubstitute.Resolve<IRunContext>();

            frameworkHandle = autoSubstitute.Resolve<IFrameworkHandle>();

            crossDomainTestExecutor = autoSubstitute.Resolve<ICrossDomainTestExecutor>();

            outputLogger = autoSubstitute.Resolve<OutputLogger>();
            var loggerFactory = autoSubstitute.Resolve<ILoggerFactory>();
            loggerFactory.CreateOutput(Arg.Any<IMessageLogger>()).Returns(outputLogger);

            executionObserver = autoSubstitute.Resolve<IExecutionObserver>();
            var executionObserverFactory = autoSubstitute.Resolve<IExecutionObserverFactory>();
            executionObserverFactory.Create(frameworkHandle).Returns(executionObserver);

            executor = autoSubstitute.Resolve<NSpecTestExecutor>();
        }

        [TearDown]
        public virtual void after_each()
        {
            autoSubstitute.Dispose();
            executor.Dispose();
        }
    }

    public class NSpecTestExecutor_when_running_sources : NSpecTestExecutor_desc_base
    {
        List<string> executedSources;

        public override void before_each()
        {
            base.before_each();

            executedSources = new List<string>();

            crossDomainTestExecutor
                .When(exc => exc.Execute(Arg.Any<string>(), executionObserver, outputLogger, outputLogger))
                .Do(callInfo =>
                {
                    var source = callInfo.Arg<string>();

                    executedSources.Add(source);
                });

            executor.RunTests(sources, runContext, frameworkHandle);
        }

        [Test]
        public void it_should_pass_message_logger()
        {
            crossDomainTestExecutor.Received().Execute(
                Arg.Any<string>(),
                Arg.Any<IExecutionObserver>(),
                outputLogger, outputLogger);
        }

        [Test]
        public void it_should_pass_execution_observer()
        {
            crossDomainTestExecutor.Received().Execute(
                Arg.Any<string>(),
                executionObserver,
                Arg.Any<IOutputLogger>(), Arg.Any<IReplayLogger>());
        }

        [Test]
        public void it_should_process_all_sources()
        {
            executedSources.ShouldBeEquivalentTo(sources);
        }
    }

    public class NSpecTestExecutor_when_running_test_cases : NSpecTestExecutor_desc_base
    {
        Dictionary<string, TestCase[]> testCaseBySource;
        IEnumerable<TestCase> testCases;
        Dictionary<string, IEnumerable<string>> executedTests;

        public NSpecTestExecutor_when_running_test_cases()
        {
            string source1 = sources[0];
            string source2 = sources[1];

            string codeFilePath1 = @".\path\to\dummy\source-code.cs";
            string codeFilePath2 = @".\other\path\to\another-source-code.cs";

            testCaseBySource = new Dictionary<string, TestCase[]>()
            {
                {
                    source1,
                    new TestCase[]
                    {
                        new TestCase(
                            "nspec. ParentSpec. ChildSpec. method context 4. child example 4A.", 
                            Constants.ExecutorUri, source1)
                        {
                            DisplayName = "nspec. ParentSpec. ChildSpec. method context 4. child example 4A.",
                            CodeFilePath = codeFilePath1,
                            LineNumber = 41,
                        },
                        new TestCase(
                            "nspec. ParentSpec. method context 1. parent example 1B.", 
                            Constants.ExecutorUri, source1)
                        {
                            DisplayName = "nspec. ParentSpec. method context 1. parent example 1B.",
                            CodeFilePath = codeFilePath1,
                            LineNumber = 21,
                        },
                        new TestCase(
                            "nspec. ParentSpec. method context 2. parent example 2A.", 
                            Constants.ExecutorUri, source1)
                        {
                            DisplayName = "nspec. ParentSpec. method context 2. parent example 2A.",
                            CodeFilePath = codeFilePath1,
                            LineNumber = 26,
                        },
                    }
                },
                {
                    source2,
                    new TestCase[]
                    {
                        new TestCase(
                            "nspec. ParentSpec. ChildSpec. method context 3. child example 3A skipped.", 
                            Constants.ExecutorUri, source2)
                        {
                            DisplayName = "nspec. ParentSpec. ChildSpec. method context 3. child example 3A skipped.",
                            // no source code info available for pending tests
                            CodeFilePath = String.Empty,
                            LineNumber = 0,
                        },
                        new TestCase(
                            "nspec. ParentSpec. ChildSpec. method context 4. child example 4A.", 
                            Constants.ExecutorUri, source2)
                        {
                            DisplayName = "nspec. ParentSpec. ChildSpec. method context 4. child example 4A.",
                            CodeFilePath = codeFilePath2,
                            LineNumber = 41,
                        },
                    }
                },
            };

            testCases = testCaseBySource
                .SelectMany(bySource => bySource.Value);
        }

        public override void before_each()
        {
            base.before_each();

            executedTests = new Dictionary<string, IEnumerable<string>>();

            crossDomainTestExecutor
                .When(exc => exc.Execute(Arg.Any<string>(), Arg.Any<IEnumerable<string>>(), executionObserver, outputLogger, outputLogger))
                .Do(callInfo =>
                {
                    var source = callInfo.Arg<string>();

                    var fullNames = callInfo.Arg<IEnumerable<string>>();

                    executedTests[source] = fullNames;
                });

            executor.RunTests(testCases, runContext, frameworkHandle);
        }

        [Test]
        public void it_should_pass_message_logger()
        {
            crossDomainTestExecutor.Received().Execute(
                Arg.Any<string>(), 
                Arg.Any<IEnumerable<string>>(),
                Arg.Any<IExecutionObserver>(), 
                outputLogger, outputLogger);
        }

        [Test]
        public void it_should_pass_execution_observer()
        {
            crossDomainTestExecutor.Received().Execute(
                Arg.Any<string>(),
                Arg.Any<IEnumerable<string>>(),
                executionObserver, 
                Arg.Any<IOutputLogger>(), Arg.Any<IReplayLogger>());
        }

        [Test]
        public void it_should_process_all_test_cases()
        {
            var expected = testCases
                .GroupBy(tc => tc.Source)
                .ToDictionary(group => group.Key, group => group.Select(tc => tc.FullyQualifiedName));

            executedTests.ShouldBeEquivalentTo(expected);
        }
    }
}
