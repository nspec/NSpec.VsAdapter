using AutofacContrib.NSubstitute;
using FluentAssertions;
using Microsoft.Reactive.Testing;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSpec.VsAdapter;
using System.Reactive.Subjects;

namespace NSpec.VsAdapter.UnitTests
{
    [TestFixture]
    [Category("NSpecTestDllNotifier")]
    public class NSpecTestDllNotifier_desc
    {
        NSpecTestDllNotifier notifier;

        AutoSubstitute autoSubstitute;
        Subject<IEnumerable<ProjectBuildInfo>> projectBuildInfoStream;
        IProjectBuildConverter projectBuildConverter;
        ITestableObserver<IEnumerable<string>> testDllPathObserver;
        IDisposable subscription;

        const string notATestDllPath = null;

        [SetUp]
        public virtual void before_each()
        {
            autoSubstitute = new AutoSubstitute();

            var projectBuildNotifier = autoSubstitute.Resolve<IProjectBuildNotifier>();
            projectBuildInfoStream = new Subject<IEnumerable<ProjectBuildInfo>>();
            projectBuildNotifier.BuildStream.Returns(projectBuildInfoStream);

            projectBuildConverter = autoSubstitute.Resolve<IProjectBuildConverter>();

            notifier = autoSubstitute.Resolve<NSpecTestDllNotifier>();

            testDllPathObserver = new TestScheduler().CreateObserver<IEnumerable<string>>();

            subscription = notifier.PathStream.Subscribe(testDllPathObserver);
        }

        [TearDown]
        public virtual void after_each()
        {
            autoSubstitute.Dispose();
            projectBuildInfoStream.Dispose();
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
                new ProjectBuildInfo(),
                new ProjectBuildInfo(),
                new ProjectBuildInfo(),
            };

            projectBuildConverter.ToTestDllPath(Arg.Any<ProjectBuildInfo>()).Returns(notATestDllPath);

            projectBuildInfoStream.OnNext(buildInfos);

            testDllPathObserver.Messages.Should().HaveCount(1);

            IEnumerable<string> testDllPaths = testDllPathObserver.Messages[0].Value.Value;

            testDllPaths.Should().BeEmpty();
        }

        [Test]
        public void it_should_notify_only_test_paths_when_some_test_is_notified()
        {
            var testBuildInfo = new ProjectBuildInfo();
            var anotherTestBuildInfo = new ProjectBuildInfo();

            var buildInfos = new[]
            {
                new ProjectBuildInfo(),
                testBuildInfo,
                anotherTestBuildInfo,
            };

            string someTestDllPath = @".\some\dummy\test\library.dll";
            string anotherTestDllPath = @".\another\dummy\test\library.dll";

            projectBuildConverter.ToTestDllPath(Arg.Any<ProjectBuildInfo>()).Returns(notATestDllPath);
            projectBuildConverter.ToTestDllPath(testBuildInfo).Returns(someTestDllPath);
            projectBuildConverter.ToTestDllPath(anotherTestBuildInfo).Returns(anotherTestDllPath);

            projectBuildInfoStream.OnNext(buildInfos);

            testDllPathObserver.Messages.Should().HaveCount(1);

            IEnumerable<string> testDllPaths = testDllPathObserver.Messages[0].Value.Value;

            testDllPaths.Should().HaveCount(2);

            testDllPaths.Should().Contain(someTestDllPath);
            testDllPaths.Should().Contain(anotherTestDllPath);
        }
    }
}
