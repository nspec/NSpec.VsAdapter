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

            var updateProjectDoneStream = Observable.Create<UpdateProjectDoneInfo>(observer =>
                {
                    uint unregisterId = VSConstants.VSCOOKIE_NIL;
                    var solutionUpdateEventSink = new SolutionUpdateEventSink(observer);

                    // TODO check result and manage failure

                    solutionBuildManager.AdviseUpdateSolutionEvents(solutionUpdateEventSink, out unregisterId);

                    Action disposeAction = () =>
                    {
                        solutionBuildManager.UnadviseUpdateSolutionEvents(unregisterId);
                    };

                    return disposeAction;
                });

            const int updateActionFailed = 0;

            var hotBuildStream = updateProjectDoneStream
                .Where(updateInfo => 
                    {
                        uint buildFlag = updateInfo.dwAction & (uint)VSSOLNBUILDUPDATEFLAGS.SBF_OPERATION_BUILD;

                        bool isABuildAction = (buildFlag != 0);

                        bool isSuccess = updateInfo.fSuccess != updateActionFailed;

                        return isSuccess && isABuildAction;
                    })
                .Select(updateInfo => new ProjectInfo() { Hierarchy = updateInfo.pHierProj })
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
            public SolutionUpdateEventSink(IObserver<UpdateProjectDoneInfo> updateProjectDoneObserver)
            {
                this.updateProjectDoneObserver = updateProjectDoneObserver;
            }

            public int OnActiveProjectCfgChange(IVsHierarchy pIVsHierarchy)
            {
                return VSConstants.S_OK;
            }

            public int UpdateProjectCfg_Begin(IVsHierarchy pHierProj, IVsCfg pCfgProj, IVsCfg pCfgSln, uint dwAction, ref int pfCancel)
            {
                return VSConstants.S_OK;
            }

            public int UpdateProjectCfg_Done(IVsHierarchy pHierProj, IVsCfg pCfgProj, IVsCfg pCfgSln, uint dwAction, int fSuccess, int fCancel)
            {
                var doneInfo = new UpdateProjectDoneInfo()
                {
                    pHierProj = pHierProj,
                    pCfgProj = pCfgProj,
                    pCfgSln = pCfgSln,
                    dwAction = dwAction,
                    fSuccess = fSuccess,
                    fCancel = fCancel,
                };

                updateProjectDoneObserver.OnNext(doneInfo);

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

            public int UpdateSolution_StartUpdate(ref int pfCancelUpdate)
            {
                return VSConstants.S_OK;
            }

            readonly IObserver<UpdateProjectDoneInfo> updateProjectDoneObserver;
        }

        class UpdateProjectDoneInfo
        {
            public IVsHierarchy pHierProj;
            public IVsCfg pCfgProj;
            public IVsCfg pCfgSln;
            public uint dwAction;
            public int fSuccess;
            public int fCancel;
        }
    }
}
