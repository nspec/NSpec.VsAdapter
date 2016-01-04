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
    public abstract class NSpecTestDllNotifier_desc_base
    {
        protected NSpecTestDllNotifier notifier;

        protected AutoSubstitute autoSubstitute;
        protected Subject<IEnumerable<ProjectInfo>> projectStream;
        protected IProjectConverter projectConverter;
        protected ITestableObserver<IEnumerable<string>> testDllPathObserver;
        protected IDisposable subscription;

        protected const string notATestDllPath = null;

        [SetUp]
        public virtual void before_each()
        {
            autoSubstitute = new AutoSubstitute();

            var projectNotifier = autoSubstitute.Resolve<IProjectNotifier>();
            projectStream = new Subject<IEnumerable<ProjectInfo>>();
            projectNotifier.ProjectStream.Returns(projectStream);

            projectConverter = autoSubstitute.Resolve<IProjectConverter>();

            projectConverter.ToTestDllPath(Arg.Any<ProjectInfo>()).Returns(notATestDllPath);

            notifier = autoSubstitute.Resolve<NSpecTestDllNotifier>();

            testDllPathObserver = new TestScheduler().CreateObserver<IEnumerable<string>>();

            subscription = notifier.PathStream.Subscribe(testDllPathObserver);
        }

        [TearDown]
        public virtual void after_each()
        {
            autoSubstitute.Dispose();
            projectStream.Dispose();
            notifier.Dispose();
            subscription.Dispose();
        }
    }

    public class NSpecTestDllNotifier_when_created : NSpecTestDllNotifier_desc_base
    {
        [Test]
        public void it_should_not_notify()
        {
            testDllPathObserver.Messages.Should().BeEmpty();
        }
    }

    public class NSpecTestDllNotifier_when_no_test_found : NSpecTestDllNotifier_desc_base
    {
        public override void before_each()
        {
            base.before_each();

            var buildInfos = new[]
            {
                new ProjectInfo(),
                new ProjectInfo(),
                new ProjectInfo(),
            };

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
    }

    public class NSpecTestDllNotifier_when_some_test_found : NSpecTestDllNotifier_desc_base
    {
        ProjectInfo projectInfo;
        ProjectInfo anotherProjectInfo;

        const string someTestDllPath = @".\some\dummy\test\library.dll";
        const string anotherTestDllPath = @".\another\dummy\test\library.dll";

        public override void before_each()
        {
            base.before_each();

            projectInfo = new ProjectInfo();
            anotherProjectInfo = new ProjectInfo();

            var buildInfos = new[]
            {
                new ProjectInfo(),
                projectInfo,
                anotherProjectInfo,
            };

            projectConverter.ToTestDllPath(projectInfo).Returns(someTestDllPath);
            projectConverter.ToTestDllPath(anotherProjectInfo).Returns(anotherTestDllPath);

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

            testDllPaths.Should().BeEquivalentTo(new string [] 
                {
                    someTestDllPath,
                    anotherTestDllPath,
                });
        }
    }
}
