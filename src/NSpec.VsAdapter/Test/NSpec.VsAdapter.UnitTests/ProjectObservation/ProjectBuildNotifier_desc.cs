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
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.UnitTests.ProjectObservation
{
    [TestFixture]
    [Category("ProjectBuildNotifier")]
    public class ProjectBuildNotifier_desc
    {
        ProjectBuildNotifier notifier;

        AutoSubstitute autoSubstitute;
        IVsUpdateSolutionEvents2 solutionUpdateEventSink;
        ITestableObserver<ProjectInfo> buildObserver;
        IDisposable subscription;
        IVsHierarchy someHierarchy;

        const int flagTrue = 1;
        const int flagFalse = 0;

        [SetUp]
        public virtual void before_each()
        {
            autoSubstitute = new AutoSubstitute();

            var testBuildManager = autoSubstitute.Resolve<IVsSolutionBuildManager2>();
            
            uint unregisterId;
            uint dummyId = 12345;

            testBuildManager.AdviseUpdateSolutionEvents(Arg.Any<IVsUpdateSolutionEvents2>(), out unregisterId)
                .Returns(callInfo =>
                {
                    solutionUpdateEventSink = callInfo.Arg<IVsUpdateSolutionEvents2>();
                    callInfo[1] = dummyId;

                    return VSConstants.S_OK;
                });

            var solutionBuildManagerProvider = autoSubstitute.Resolve<ISolutionBuildManagerProvider>();
            solutionBuildManagerProvider.Provide().Returns(testBuildManager);

            notifier = autoSubstitute.Resolve<ProjectBuildNotifier>();

            buildObserver = new TestScheduler().CreateObserver<ProjectInfo>();

            subscription = notifier.BuildStream.Subscribe(buildObserver);

            someHierarchy = autoSubstitute.Resolve<IVsHierarchy>();
        }

        [TearDown]
        public virtual void after_each()
        {
            autoSubstitute.Dispose();
            notifier.Dispose();
            subscription.Dispose();
        }

        [Test]
        public void it_should_not_notify_when_created()
        {
            buildObserver.Messages.Should().BeEmpty();
        }

        [Test]
        public void it_should_not_notify_when_not_a_build_action()
        {
            uint updateAction = (uint)VSSOLNBUILDUPDATEFLAGS.SBF_OPERATION_CLEAN;

            solutionUpdateEventSink.UpdateProjectCfg_Done(someHierarchy, null, null, updateAction, flagTrue, flagFalse);

            buildObserver.Messages.Should().BeEmpty();
        }

        [Test]
        public void it_should_not_notify_when_build_action_failed()
        {
            uint updateAction = (uint)(VSSOLNBUILDUPDATEFLAGS.SBF_OPERATION_BUILD 
                | VSSOLNBUILDUPDATEFLAGS.SBF_OPERATION_FORCE_UPDATE);

            solutionUpdateEventSink.UpdateProjectCfg_Done(someHierarchy, null, null, updateAction, flagFalse, flagFalse);

            buildObserver.Messages.Should().BeEmpty();
        }

        [Test]
        public void it_should_notify_when_build_action_succeeded()
        {
            uint updateAction = (uint)VSSOLNBUILDUPDATEFLAGS.SBF_OPERATION_BUILD;

            solutionUpdateEventSink.UpdateProjectCfg_Done(someHierarchy, null, null, updateAction, flagTrue, flagFalse);

            buildObserver.Messages.Should().HaveCount(1);

            buildObserver.Messages.Single().Value.Value.Hierarchy.Should().Be(someHierarchy);
        }
    }
}
