using AutofacContrib.NSubstitute;
using FluentAssertions;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.Shell.Interop;
using NSpec.VsAdapter.ProjectObservation;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.UnitTests.ProjectObservation
{
    [TestFixture]
    [Category("ProjectNotifier")]
    public class ProjectNotifier_desc
    {
        ProjectNotifier notifier;

        AutoSubstitute autoSubstitute;
        Subject<SolutionInfo> solutionOpenedStream;
        Subject<Unit> solutionClosingStream;
        Subject<ProjectInfo> projectAddedStream;
        Subject<ProjectInfo> projectRemovingtream;
        IProjectEnumerator projectEnumerator;
        ITestableObserver<IEnumerable<ProjectInfo>> testProjectObserver;
        IDisposable subscription;

        SolutionInfo someSolution;
        ProjectInfo[] someProjectInfos;

        [SetUp]
        public virtual void before_each()
        {
            autoSubstitute = new AutoSubstitute();

            solutionOpenedStream = new Subject<SolutionInfo>();
            solutionClosingStream = new Subject<Unit>();
            projectAddedStream = new Subject<ProjectInfo>();
            projectRemovingtream = new Subject<ProjectInfo>();

            var solutionNotifier = autoSubstitute.Resolve<ISolutionNotifier>();
            solutionNotifier.SolutionOpenedStream.Returns(solutionOpenedStream);
            solutionNotifier.SolutionClosingStream.Returns(solutionClosingStream);
            solutionNotifier.ProjectAddedStream.Returns(projectAddedStream);
            solutionNotifier.ProjectRemovingtream.Returns(projectRemovingtream);

            projectEnumerator = autoSubstitute.Resolve<IProjectEnumerator>();

            notifier = autoSubstitute.Resolve<ProjectNotifier>();

            testProjectObserver = new TestScheduler().CreateObserver<IEnumerable<ProjectInfo>>();

            subscription = notifier.ProjectStream.Subscribe(testProjectObserver);

            someSolution = new SolutionInfo();

            someProjectInfos = new []
                {
                    new ProjectInfo(),
                    new ProjectInfo(),
                    new ProjectInfo(),
                };
        }

        [TearDown]
        public virtual void after_each()
        {
            autoSubstitute.Dispose();
            solutionOpenedStream.Dispose();
            solutionClosingStream.Dispose();
            projectAddedStream.Dispose();
            projectRemovingtream.Dispose();
            notifier.Dispose();
            subscription.Dispose();
        }

        [Test]
        public void it_should_not_notify_when_created()
        {
            testProjectObserver.Messages.Should().BeEmpty();
        }

        [Test]
        public void it_should_notify_projects_when_solution_opened()
        {
            projectEnumerator.GetLoadedProjects(someSolution).Returns(someProjectInfos);

            solutionOpenedStream.OnNext(someSolution);

            testProjectObserver.Messages.Should().HaveCount(1);

            testProjectObserver.Messages.Single().Value.Value.Should().BeSameAs(someProjectInfos);
        }

        [Test]
        public void it_should_notify_projects_when_solution_changes()
        {
            var someProject = new ProjectInfo();
            var changedProjectInfos = someProjectInfos.ToList();
            changedProjectInfos.Add(someProject);

            projectEnumerator.GetLoadedProjects(someSolution).Returns(someProjectInfos);

            solutionOpenedStream.OnNext(someSolution);

            testProjectObserver.Messages.Clear();

            projectEnumerator.GetLoadedProjects(someSolution).Returns(changedProjectInfos);

            projectAddedStream.OnNext(someProject);

            testProjectObserver.Messages.Should().HaveCount(1);

            testProjectObserver.Messages.Single().Value.Value.Should().BeSameAs(changedProjectInfos);
        }

        [Test]
        public void it_should_notify_no_projects_when_solution_closed()
        {
            projectEnumerator.GetLoadedProjects(someSolution).Returns(someProjectInfos);

            solutionOpenedStream.OnNext(someSolution);

            testProjectObserver.Messages.Clear();

            solutionClosingStream.OnNext(Unit.Default);

            testProjectObserver.Messages.Should().HaveCount(1);

            testProjectObserver.Messages.Single().Value.Value.Should().BeEmpty();
        }
    }
}
