using AutofacContrib.NSubstitute;
using FluentAssertions;
using Microsoft.Reactive.Testing;
using NSpec.VsAdapter.ProjectObservation;
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
    [Category("NSpecTestDllNotifier")]
    public class NSpecTestDllNotifier_desc
    {
        NSpecTestDllNotifier notifier;

        AutoSubstitute autoSubstitute;
        Subject<IEnumerable<ProjectInfo>> projectInfoStream;
        IProjectConverter projectConverter;
        ITestableObserver<IEnumerable<string>> testDllPathObserver;
        IDisposable subscription;

        const string notATestDllPath = null;

        [SetUp]
        public virtual void before_each()
        {
            autoSubstitute = new AutoSubstitute();

            var projectNotifier = autoSubstitute.Resolve<IProjectNotifier>();
            projectInfoStream = new Subject<IEnumerable<ProjectInfo>>();
            projectNotifier.ProjectStream.Returns(projectInfoStream);

            projectConverter = autoSubstitute.Resolve<IProjectConverter>();

            notifier = autoSubstitute.Resolve<NSpecTestDllNotifier>();

            testDllPathObserver = new TestScheduler().CreateObserver<IEnumerable<string>>();

            subscription = notifier.PathStream.Subscribe(testDllPathObserver);
        }

        [TearDown]
        public virtual void after_each()
        {
            autoSubstitute.Dispose();
            projectInfoStream.Dispose();
            notifier.Dispose();
            subscription.Dispose();
        }

        [Test]
        public void it_should_not_notify_when_created()
        {
            testDllPathObserver.Messages.Should().BeEmpty();
        }

        [Test]
        public void it_should_notify_empty_path_list_when_no_test_is_notified()
        {
            var buildInfos = new []
            {
                new ProjectInfo(),
                new ProjectInfo(),
                new ProjectInfo(),
            };

            projectConverter.ToTestDllPath(Arg.Any<ProjectInfo>()).Returns(notATestDllPath);

            projectInfoStream.OnNext(buildInfos);

            testDllPathObserver.Messages.Should().HaveCount(1);

            IEnumerable<string> testDllPaths = testDllPathObserver.Messages[0].Value.Value;

            testDllPaths.Should().BeEmpty();
        }

        [Test]
        public void it_should_notify_only_test_paths_when_some_test_is_notified()
        {
            var projectInfo = new ProjectInfo();
            var anotherProjectInfo = new ProjectInfo();

            var buildInfos = new[]
            {
                new ProjectInfo(),
                projectInfo,
                anotherProjectInfo,
            };

            string someTestDllPath = @".\some\dummy\test\library.dll";
            string anotherTestDllPath = @".\another\dummy\test\library.dll";

            projectConverter.ToTestDllPath(Arg.Any<ProjectInfo>()).Returns(notATestDllPath);
            projectConverter.ToTestDllPath(projectInfo).Returns(someTestDllPath);
            projectConverter.ToTestDllPath(anotherProjectInfo).Returns(anotherTestDllPath);

            projectInfoStream.OnNext(buildInfos);

            testDllPathObserver.Messages.Should().HaveCount(1);

            IEnumerable<string> testDllPaths = testDllPathObserver.Messages[0].Value.Value;

            testDllPaths.Should().HaveCount(2);

            testDllPaths.Should().Contain(someTestDllPath);
            testDllPaths.Should().Contain(anotherTestDllPath);
        }
    }
}
