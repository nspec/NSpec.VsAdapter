using AutofacContrib.NSubstitute;
using FluentAssertions;
using Microsoft.Reactive.Testing;
using NSpec.VsAdapter.ProjectObservation;
using NSpec.VsAdapter.ProjectObservation.Projects;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.UnitTests.ProjectObservation
{
    [TestFixture]
    [Category("TestBinaryNotifier")]
    public abstract class TestBinaryNotifier_desc_base
    {
        protected TestBinaryNotifier notifier;

        protected AutoSubstitute autoSubstitute;
        protected Subject<IEnumerable<ProjectInfo>> projectStream;
        protected Subject<ProjectInfo> buildStream;
        protected IProjectConverter projectConverter;
        protected ITestableObserver<IEnumerable<string>> testDllPathObserver;
        protected IDisposable subscription;

        protected readonly ProjectInfo someProjectInfo;
        protected readonly ProjectInfo testProjectInfo;
        protected readonly ProjectInfo anotherTestProjectInfo;
        protected readonly ProjectInfo[] buildInfos;

        protected const string notATestDllPath = null;

        public TestBinaryNotifier_desc_base()
        {
            someProjectInfo = new ProjectInfo();
            testProjectInfo = new ProjectInfo();
            anotherTestProjectInfo = new ProjectInfo();

            buildInfos = new[]
            {
                someProjectInfo,
                testProjectInfo,
                anotherTestProjectInfo,
            };
        }

        [SetUp]
        public virtual void before_each()
        {
            autoSubstitute = new AutoSubstitute();

            var projectNotifier = autoSubstitute.Resolve<IProjectNotifier>();
            projectStream = new Subject<IEnumerable<ProjectInfo>>();
            projectNotifier.ProjectStream.Returns(projectStream);

            var buildNotifier = autoSubstitute.Resolve<IProjectBuildNotifier>();
            buildStream = new Subject<ProjectInfo>();
            buildNotifier.BuildStream.Returns(buildStream);

            projectConverter = autoSubstitute.Resolve<IProjectConverter>();
            projectConverter.ToTestDllPath(Arg.Any<ProjectInfo>()).Returns(notATestDllPath);

            notifier = autoSubstitute.Resolve<TestBinaryNotifier>();

            testDllPathObserver = new TestScheduler().CreateObserver<IEnumerable<string>>();

            subscription = notifier.PathStream.Subscribe(testDllPathObserver);
        }

        [TearDown]
        public virtual void after_each()
        {
            autoSubstitute.Dispose();
            projectStream.Dispose();
            buildStream.Dispose();
            notifier.Dispose();
            subscription.Dispose();
        }
    }

    public class TestBinaryNotifier_when_created : TestBinaryNotifier_desc_base
    {
        [Test]
        public void it_should_not_notify()
        {
            testDllPathObserver.Messages.Should().BeEmpty();
        }
    }

    public class TestBinaryNotifier_when_no_test_found : TestBinaryNotifier_desc_base
    {
        public override void before_each()
        {
            base.before_each();

            projectStream.OnNext(buildInfos);
        }

        [Test]
        public void it_should_notify_once()
        {
            testDllPathObserver.Messages.Should().HaveCount(1);
        }

        [Test]
        public void it_should_notify_empty_path_list()
        {
            IEnumerable<string> testDllPaths = testDllPathObserver.Messages.Single().Value.Value;

            testDllPaths.Should().BeEmpty();
        }

        [Test]
        public void it_should_not_notify_when_project_builds()
        {
            testDllPathObserver.Messages.Clear();

            buildStream.OnNext(someProjectInfo);

            testDllPathObserver.Messages.Should().BeEmpty();
        }
    }

    public class TestBinaryNotifier_when_some_test_found : TestBinaryNotifier_desc_base
    {
        const string someTestDllPath = @".\some\dummy\test\library.dll";
        const string anotherTestDllPath = @".\another\dummy\test\library.dll";

        public override void before_each()
        {
            base.before_each();

            projectConverter.ToTestDllPath(testProjectInfo).Returns(someTestDllPath);
            projectConverter.ToTestDllPath(anotherTestProjectInfo).Returns(anotherTestDllPath);

            projectStream.OnNext(buildInfos);
        }

        [Test]
        public void it_should_notify_once()
        {
            testDllPathObserver.Messages.Should().HaveCount(1);
        }

        [Test]
        public void it_should_notify_only_test_paths()
        {
            IEnumerable<string> testDllPaths = testDllPathObserver.Messages.Single().Value.Value;

            testDllPaths.Should().BeEquivalentTo(new string[] 
                {
                    someTestDllPath,
                    anotherTestDllPath,
                });
        }

        [Test]
        public void it_should_not_notify_when_non_test_project_builds()
        {
            testDllPathObserver.Messages.Clear();

            buildStream.OnNext(someProjectInfo);

            testDllPathObserver.Messages.Should().BeEmpty();
        }

        [Test]
        public void it_should_notify_when_test_project_builds()
        {
            testDllPathObserver.Messages.Clear();

            buildStream.OnNext(testProjectInfo);

            testDllPathObserver.Messages.Should().HaveCount(1);
        }
    }
}
