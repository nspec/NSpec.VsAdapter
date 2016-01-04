using AutofacContrib.NSubstitute;
using FluentAssertions;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using NSpec.VsAdapter.ProjectObservation;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.UnitTests.ProjectObservation
{
    [TestFixture]
    [Category("SolutionNotifier")]
    class SolutionNotifier_desc
    {
        SolutionNotifier notifier;

        AutoSubstitute autoSubstitute;
        IVsSolution someSolution;
        IVsSolutionEvents solutionEventSink;
        ITestableObserver<SolutionInfo> solutionOpenedObserver;
        ITestableObserver<Unit> solutionClosingObserver;
        ITestableObserver<ProjectInfo> projectAddedObserver;
        ITestableObserver<ProjectInfo> projectRemovingObserver;
        CompositeDisposable disposables;
        IVsHierarchy someHierarchy;

        [SetUp]
        public virtual void before_each()
        {
            autoSubstitute = new AutoSubstitute();

            someSolution = autoSubstitute.Resolve<IVsSolution>();

            uint unregisterToken = VSConstants.VSCOOKIE_NIL;
            uint dummyToken = 12345;

            someSolution.AdviseSolutionEvents(Arg.Any<IVsSolutionEvents>(), out unregisterToken)
                .Returns(callInfo =>
                {
                    solutionEventSink = callInfo.Arg<IVsSolutionEvents>();
                    callInfo[1] = dummyToken;

                    return VSConstants.S_OK;
                });

            var solutionProvider = autoSubstitute.Resolve<ISolutionProvider>();
            solutionProvider.Provide().Returns(someSolution);

            notifier = autoSubstitute.Resolve<SolutionNotifier>();

            var testScheduler = new TestScheduler();

            solutionOpenedObserver = testScheduler.CreateObserver<SolutionInfo>();
            solutionClosingObserver = testScheduler.CreateObserver<Unit>();
            projectAddedObserver = testScheduler.CreateObserver<ProjectInfo>();
            projectRemovingObserver = testScheduler.CreateObserver<ProjectInfo>();

            disposables = new CompositeDisposable();

            notifier.SolutionOpenedStream.Subscribe(solutionOpenedObserver).DisposeWith(disposables);
            notifier.SolutionClosingStream.Subscribe(solutionClosingObserver).DisposeWith(disposables);
            notifier.ProjectAddedStream.Subscribe(projectAddedObserver).DisposeWith(disposables);
            notifier.ProjectRemovingtream.Subscribe(projectRemovingObserver).DisposeWith(disposables);

            someHierarchy = autoSubstitute.Resolve<IVsHierarchy>();
        }

        [TearDown]
        public virtual void after_each()
        {
            autoSubstitute.Dispose();
            notifier.Dispose();
            disposables.Dispose();
        }

        [Test]
        public void it_should_not_notify_when_created()
        {
            solutionOpenedObserver.Messages.Should().BeEmpty();
            solutionClosingObserver.Messages.Should().BeEmpty();
            projectAddedObserver.Messages.Should().BeEmpty();
            projectRemovingObserver.Messages.Should().BeEmpty();
        }

        [Test]
        public void it_should_notify_when_solution_created()
        {
            int created = VSConstants.S_OK;

            solutionEventSink.OnAfterOpenSolution(new Object(), created);

            solutionOpenedObserver.Messages.Should().HaveCount(1);
            solutionOpenedObserver.Messages.Single().Value.Value.Solution.Should().Be(someSolution);

            solutionClosingObserver.Messages.Should().BeEmpty();
            projectAddedObserver.Messages.Should().BeEmpty();
            projectRemovingObserver.Messages.Should().BeEmpty();
        }

        [Test]
        public void it_should_not_notify_when_solution_opened()
        {
            int created = VSConstants.S_FALSE;

            solutionEventSink.OnAfterOpenSolution(new Object(), created);

            solutionOpenedObserver.Messages.Should().HaveCount(1);
            solutionOpenedObserver.Messages.Single().Value.Value.Solution.Should().Be(someSolution);

            solutionClosingObserver.Messages.Should().BeEmpty();
            projectAddedObserver.Messages.Should().BeEmpty();
            projectRemovingObserver.Messages.Should().BeEmpty();
        }

        [Test]
        public void it_should_notify_when_solution_closing()
        {
            solutionEventSink.OnBeforeCloseSolution(new Object());

            solutionOpenedObserver.Messages.Should().BeEmpty();
            solutionClosingObserver.Messages.Should().HaveCount(1);
            projectAddedObserver.Messages.Should().BeEmpty();
            projectRemovingObserver.Messages.Should().BeEmpty();
        }

        [Test]
        public void it_should_notify_when_project_added()
        {
            int added = VSConstants.S_OK;

            solutionEventSink.OnAfterOpenProject(someHierarchy, added);

            solutionOpenedObserver.Messages.Should().BeEmpty();
            solutionClosingObserver.Messages.Should().BeEmpty();
            projectAddedObserver.Messages.Should().HaveCount(1);
            projectAddedObserver.Messages.Single().Value.Value.Hierarchy.Should().Be(someHierarchy);

            projectRemovingObserver.Messages.Should().BeEmpty();
        }

        [Test]
        public void it_should_not_notify_when_project_added_while_solution_loads()
        {
            int added = VSConstants.S_FALSE;

            solutionEventSink.OnAfterOpenProject(someHierarchy, added);

            solutionOpenedObserver.Messages.Should().BeEmpty();
            solutionClosingObserver.Messages.Should().BeEmpty();
            projectAddedObserver.Messages.Should().BeEmpty();
            projectRemovingObserver.Messages.Should().BeEmpty();
        }

        [Test]
        public void it_should_notify_when_project_closing()
        {
            int removed = VSConstants.S_OK;

            solutionEventSink.OnBeforeCloseProject(someHierarchy, removed);

            solutionOpenedObserver.Messages.Should().BeEmpty();
            solutionClosingObserver.Messages.Should().BeEmpty();
            projectAddedObserver.Messages.Should().BeEmpty();
            projectRemovingObserver.Messages.Should().HaveCount(1);
            projectRemovingObserver.Messages.Single().Value.Value.Hierarchy.Should().Be(someHierarchy);
        }

        [Test]
        public void it_should_not_notify_when_project_closing_while_solution_closes()
        {
            int removed = VSConstants.S_FALSE;

            solutionEventSink.OnBeforeCloseProject(someHierarchy, removed);

            solutionOpenedObserver.Messages.Should().BeEmpty();
            solutionClosingObserver.Messages.Should().BeEmpty();
            projectAddedObserver.Messages.Should().BeEmpty();
            projectRemovingObserver.Messages.Should().BeEmpty();
        }
    }
}
