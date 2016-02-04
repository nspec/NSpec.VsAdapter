using AutofacContrib.NSubstitute;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using NSpec.VsAdapter.Execution;
using NSpec.VsAdapter.Logging;
using NSpec.VsAdapter.Settings;
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
    [Category("MultiSourceTestExecutor")]
    public abstract class MultiSourceTestExecutor_desc_base
    {
        protected MultiSourceTestExecutor executor;

        protected AutoSubstitute autoSubstitute;
        protected IBinaryTestExecutor binaryTestExecutor;
        protected IProgressRecorderFactory progressRecorderFactory;
        protected IProgressRecorder progressRecorder;
        protected ISettingsRepository settingsRepository;
        protected ILoggerFactory loggerFactory;
        protected IOutputLogger outputLogger;
        protected IFrameworkHandle frameworkHandle;
        protected List<string> actualSources;
        protected string[] sources;

        public MultiSourceTestExecutor_desc_base()
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

            frameworkHandle = autoSubstitute.Resolve<IFrameworkHandle>();

            binaryTestExecutor = autoSubstitute.Resolve<IBinaryTestExecutor>();

            progressRecorder = autoSubstitute.Resolve<IProgressRecorder>();
            progressRecorderFactory = autoSubstitute.Resolve<IProgressRecorderFactory>();
            progressRecorderFactory.Create(frameworkHandle).Returns(progressRecorder);

            settingsRepository = autoSubstitute.Resolve<ISettingsRepository>();

            outputLogger = autoSubstitute.Resolve<IOutputLogger>();
            loggerFactory = autoSubstitute.Resolve<ILoggerFactory>();
            loggerFactory.CreateOutput(null, null).ReturnsForAnyArgs(outputLogger);

            actualSources = new List<string>();

            progressRecorder.BinaryPath = Arg.Do<string>(path => actualSources.Add(path));
        }

        [TearDown]
        public virtual void after_each()
        {
            autoSubstitute.Dispose();
        }
    }

    public class MultiSourceTestExecutor_when_running_sources : MultiSourceTestExecutor_desc_base
    {
        List<string> executedSources;

        public override void before_each()
        {
            base.before_each();

            executedSources = new List<string>();

            binaryTestExecutor
                .When(exc => exc.Execute(
                    Arg.Any<string>(), 
                    progressRecorder, 
                    outputLogger, 
                    Arg.Any<ICrossDomainLogger>()))
                .Do(callInfo =>
                {
                    var source = callInfo.Arg<string>();

                    executedSources.Add(source);
                });

            executor = new MultiSourceTestExecutor(sources, binaryTestExecutor, progressRecorderFactory, settingsRepository, loggerFactory);

            executor.RunTests(frameworkHandle, autoSubstitute.Resolve<IRunContext>());
        }

        [Test]
        public void it_should_pass_binary_path()
        {
            actualSources.ShouldBeEquivalentTo(sources);
        }

        [Test]
        public void it_should_pass_message_logger()
        {
            binaryTestExecutor.Received().Execute(
                Arg.Any<string>(),
                Arg.Any<IProgressRecorder>(),
                outputLogger,
                Arg.Any<ICrossDomainLogger>());
        }

        [Test]
        public void it_should_pass_execution_observer()
        {
            binaryTestExecutor.Received().Execute(
                Arg.Any<string>(),
                progressRecorder,
                Arg.Any<IOutputLogger>(), Arg.Any<ICrossDomainLogger>());
        }

        [Test]
        public void it_should_process_all_sources()
        {
            executedSources.ShouldBeEquivalentTo(sources);
        }
    }

    public class MultiSourceTestExecutor_when_running_test_cases : MultiSourceTestExecutor_desc_base
    {
        Dictionary<string, TestCase[]> testCaseBySource;
        IEnumerable<TestCase> testCases;
        Dictionary<string, IEnumerable<string>> executedTests;

        public MultiSourceTestExecutor_when_running_test_cases()
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

            binaryTestExecutor
                .When(exc => exc.Execute(
                    Arg.Any<string>(), 
                    Arg.Any<IEnumerable<string>>(), 
                    progressRecorder, 
                    outputLogger,
                    Arg.Any<ICrossDomainLogger>()))
                .Do(callInfo =>
                {
                    var source = callInfo.Arg<string>();

                    var fullNames = callInfo.Arg<IEnumerable<string>>();

                    executedTests[source] = fullNames;
                });

            executor = new MultiSourceTestExecutor(testCases, binaryTestExecutor, progressRecorderFactory, settingsRepository, loggerFactory);

            executor.RunTests(frameworkHandle, autoSubstitute.Resolve<IRunContext>());
        }

        [Test]
        public void it_should_pass_binary_path()
        {
            actualSources.ShouldBeEquivalentTo(sources);
        }

        [Test]
        public void it_should_pass_message_logger()
        {
            binaryTestExecutor.Received().Execute(
                Arg.Any<string>(), 
                Arg.Any<IEnumerable<string>>(),
                Arg.Any<IProgressRecorder>(), 
                outputLogger,
                Arg.Any<ICrossDomainLogger>());
        }

        [Test]
        public void it_should_pass_execution_observer()
        {
            binaryTestExecutor.Received().Execute(
                Arg.Any<string>(),
                Arg.Any<IEnumerable<string>>(),
                progressRecorder, 
                Arg.Any<IOutputLogger>(), Arg.Any<ICrossDomainLogger>());
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

    public class MultiSourceTestExecutor_when_canceling_run : MultiSourceTestExecutor_desc_base
    {
        [Test]
        [Ignore("Cannot figure out how to block & sync to inner RunTests loop")]
        public void it_should_quit_execution()
        {
            throw new NotImplementedException();
        }
    }
}
