using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;

namespace NSpec.VsAdapter.ProjectObservation
{
    public class ProjectBuildNotifier : IProjectBuildNotifier, IDisposable
    {
        public ProjectBuildNotifier(ISolutionBuildManagerProvider solutionBuildManagerProvider)
        {
            var solutionBuildManager = solutionBuildManagerProvider.Provide();

            var solutionUpdateEventStream = Observable.Create<SolutionUpdateEventInfo>(observer =>
                {
                    uint unregisterToken = VSConstants.VSCOOKIE_NIL;
                    var solutionUpdateEventSink = new SolutionUpdateEventSink(observer);

                    // TODO check result and manage failure

                    solutionBuildManager.AdviseUpdateSolutionEvents(solutionUpdateEventSink, out unregisterToken);

                    Action disposeAction = () =>
                    {
                        solutionBuildManager.UnadviseUpdateSolutionEvents(unregisterToken);
                    };

                    return disposeAction;
                });

            var hotBuildStream = solutionUpdateEventStream
                .Where(eventInfo => 
                    eventInfo.Reason == SolutionUpdateEventReason.ProjectBuildFinished && eventInfo.Success)
                .Select(eventInfo => new ProjectInfo() { Hierarchy = eventInfo.ProjectHierarchy })
                .Replay(0);

            hotBuildStream.Connect().DisposeWith(disposables);

            BuildStream = hotBuildStream;
        }

        public IObservable<ProjectInfo> BuildStream { get; private set; }

        public void Dispose()
        {
            disposables.Dispose();
        }

        readonly CompositeDisposable disposables = new CompositeDisposable();

        class SolutionUpdateEventSink : IVsUpdateSolutionEvents2
        {
            public SolutionUpdateEventSink(IObserver<SolutionUpdateEventInfo> updateEventObserver)
            {
                this.updateEventObserver = updateEventObserver;
            }

            // Solution update events

            public int UpdateSolution_StartUpdate(ref int pfCancelUpdate)
            {
                return VSConstants.S_OK;
            }

            public int UpdateSolution_Begin(ref int pfCancelUpdate)
            {
                return VSConstants.S_OK;
            }

            public int UpdateSolution_Cancel()
            {
                return VSConstants.S_OK;
            }

            public int UpdateSolution_Done(int fSucceeded, int fModified, int fCancelCommand)
            {
                return VSConstants.S_OK;
            }

            // Project update events

            public int OnActiveProjectCfgChange(IVsHierarchy pIVsHierarchy)
            {
                return VSConstants.S_OK;
            }

            public int UpdateProjectCfg_Begin(IVsHierarchy pHierProj, IVsCfg pCfgProj, IVsCfg pCfgSln, uint dwAction, ref int pfCancel)
            {
                const bool isSuccess = true;

                OnProjectUpdate(true, pHierProj, dwAction, isSuccess);

                return VSConstants.S_OK;
            }

            public int UpdateProjectCfg_Done(IVsHierarchy pHierProj, IVsCfg pCfgProj, IVsCfg pCfgSln, uint dwAction, int fSuccess, int fCancel)
            {
                const int updateActionFailed = 0;
                bool isSuccess = (fSuccess != updateActionFailed);

                OnProjectUpdate(false, pHierProj, dwAction, isSuccess);

                return VSConstants.S_OK;
            }

            void OnProjectUpdate(bool begin, IVsHierarchy hierarchy, uint action, bool isSuccess)
            {
                var updateAction = new ProjectUpdateAction(action);

                if (updateAction.IsBuild)
                {
                    var reason = (begin ? 
                        SolutionUpdateEventReason.ProjectBuildStarted : 
                        SolutionUpdateEventReason.ProjectBuildFinished);

                    var eventInfo = new SolutionUpdateEventInfo()
                    {
                        Reason = reason,
                        Success = isSuccess,
                        ProjectHierarchy = hierarchy,
                    };

                    updateEventObserver.OnNext(eventInfo);
                }
                else if (updateAction.IsClean)
                {
                    var reason = (begin ?
                        SolutionUpdateEventReason.ProjectCleanStarted :
                        SolutionUpdateEventReason.ProjectCleanFinished);

                    var eventInfo = new SolutionUpdateEventInfo()
                    {
                        Reason = reason,
                        Success = isSuccess,
                        ProjectHierarchy = hierarchy,
                    };

                    updateEventObserver.OnNext(eventInfo);
                }
            }

            readonly IObserver<SolutionUpdateEventInfo> updateEventObserver;
        }

        class ProjectUpdateAction
        {
            public ProjectUpdateAction(uint action)
            {
                uint buildFlag = action & (uint)VSSOLNBUILDUPDATEFLAGS.SBF_OPERATION_BUILD;
                IsBuild = (buildFlag != 0);

                uint cleanFlag = action & (uint)VSSOLNBUILDUPDATEFLAGS.SBF_OPERATION_CLEAN;
                IsClean = (cleanFlag != 0);
            }

            public bool IsBuild { get; private set; }

            public bool IsClean { get; private set; }
        }

        class SolutionUpdateEventInfo
        {
            public SolutionUpdateEventReason Reason;

            public IVsHierarchy ProjectHierarchy;

            public bool Success;
        }

        enum SolutionUpdateEventReason
        {
            Unassigned,

            ProjectBuildStarted,
            ProjectBuildFinished,
            ProjectCleanStarted,
            ProjectCleanFinished,
        }
    }
}
