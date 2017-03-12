using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace NSpec.VsAdapter.ProjectObservation.Solution
{
    public class SolutionNotifier : ISolutionNotifier, IDisposable
    {
        public SolutionNotifier(ISolutionProvider solutionProvider)
        {
            var solution = solutionProvider.Provide();

            var solutionEventStream = Observable.Create<SolutionEventInfo>(observer =>
                {
                    uint unregisterToken = VSConstants.VSCOOKIE_NIL;
                    var solutionEventSink = new SolutionEventSink(observer);

                    // TODO check result and manage failure

                    solution.AdviseSolutionEvents(solutionEventSink, out unregisterToken);

                    Action disposeAction = () =>
                    {
                        solution.UnadviseSolutionEvents(unregisterToken);
                    };

                    return disposeAction;
                })
                .Publish() // this will be subscribed multiple times: avoid re-subscription side-effect
                .RefCount();

            {
                var hotSolutionOpenedStream = solutionEventStream
                    .Where(eventInfo => eventInfo.Reason == SolutionEventReason.SolutionOpened)
                    .Select(_ => new SolutionInfo() { Solution = solution })
                    .Replay(0);

                hotSolutionOpenedStream.Connect().DisposeWith(disposables);

                SolutionOpenedStream = hotSolutionOpenedStream;
            }

            {
                var hotSolutionClosingStream = solutionEventStream
                    .Where(eventInfo => eventInfo.Reason == SolutionEventReason.SolutionClosing)
                    .Select(_ => Unit.Default)
                    .Replay(0);

                hotSolutionClosingStream.Connect().DisposeWith(disposables);

                SolutionClosingStream = hotSolutionClosingStream;
            }

            {
                var hotProjectAddedStream = solutionEventStream
                    .Where(eventInfo => eventInfo.Reason == SolutionEventReason.ProjectAdded)
                    .Select(eventInfo => new ProjectInfo() { Hierarchy = eventInfo.ProjectHierarchy })
                    .Replay(0);

                hotProjectAddedStream.Connect().DisposeWith(disposables);

                ProjectAddedStream = hotProjectAddedStream;
            }

            {
                var hotProjectRemovingtream = solutionEventStream
                    .Where(eventInfo => eventInfo.Reason == SolutionEventReason.ProjectRemoving)
                    .Select(eventInfo => new ProjectInfo() { Hierarchy = eventInfo.ProjectHierarchy })
                    .Replay(0);

                hotProjectRemovingtream.Connect().DisposeWith(disposables);

                ProjectRemovingtream = hotProjectRemovingtream;
            }
        }

        public IObservable<SolutionInfo> SolutionOpenedStream { get; private set; }

        public IObservable<Unit> SolutionClosingStream { get; private set; }

        public IObservable<ProjectInfo> ProjectAddedStream { get; private set; }

        public IObservable<ProjectInfo> ProjectRemovingtream { get; private set; }

        public void Dispose()
        {
            disposables.Dispose();
        }

        readonly CompositeDisposable disposables = new CompositeDisposable();

        class SolutionEventSink : IVsSolutionEvents
        {
            public SolutionEventSink(IObserver<SolutionEventInfo> eventObserver)
            {
                this.eventObserver = eventObserver;
            }

            // Solution events

            public int OnAfterOpenSolution(object pUnkReserved, int fNewSolution)
            {
                eventObserver.OnNext(new SolutionEventInfo()
                {
                    Reason = SolutionEventReason.SolutionOpened,
                });

                return VSConstants.S_OK;
            }

            public int OnQueryCloseSolution(object pUnkReserved, ref int pfCancel)
            {
                return VSConstants.S_OK;
            }

            public int OnBeforeCloseSolution(object pUnkReserved)
            {
                eventObserver.OnNext(new SolutionEventInfo()
                {
                    Reason = SolutionEventReason.SolutionClosing,
                });

                return VSConstants.S_OK;
            }

            public int OnAfterCloseSolution(object pUnkReserved)
            {
                return VSConstants.S_OK;
            }

            // Project events

            public int OnAfterOpenProject(IVsHierarchy pHierarchy, int fAdded)
            {
                bool solutionOpening = (fAdded == VSConstants.S_FALSE);

                // ignore projects being added while solution is opening
                if (! solutionOpening)
                {
                    eventObserver.OnNext(new SolutionEventInfo()
                    {
                        Reason = SolutionEventReason.ProjectAdded,
                        ProjectHierarchy = pHierarchy,
                    });
                }

                return VSConstants.S_OK;
            }

            public int OnQueryUnloadProject(IVsHierarchy pRealHierarchy, ref int pfCancel)
            {
                return VSConstants.S_OK;
            }

            public int OnBeforeUnloadProject(IVsHierarchy pRealHierarchy, IVsHierarchy pStubHierarchy)
            {
                return VSConstants.S_OK;
            }

            public int OnAfterLoadProject(IVsHierarchy pStubHierarchy, IVsHierarchy pRealHierarchy)
            {
                return VSConstants.S_OK;
            }

            public int OnQueryCloseProject(IVsHierarchy pHierarchy, int fRemoving, ref int pfCancel)
            {
                return VSConstants.S_OK;
            }

            public int OnBeforeCloseProject(IVsHierarchy pHierarchy, int fRemoved)
            {
                bool solutionClosing = (fRemoved == VSConstants.S_FALSE);

                // ignore projects being removed while solution is closing
                if (!solutionClosing)
                {
                    eventObserver.OnNext(new SolutionEventInfo()
                    {
                        Reason = SolutionEventReason.ProjectRemoving,
                        ProjectHierarchy = pHierarchy,
                    });
                }

                return VSConstants.S_OK;
            }

            readonly IObserver<SolutionEventInfo> eventObserver;
        }

        class SolutionEventInfo
        {
            public SolutionEventReason Reason;

            public IVsHierarchy ProjectHierarchy;
        }

        enum SolutionEventReason
        {
            Unassigned,

            SolutionClosing,
            ProjectAdded,
            SolutionOpened,
            ProjectRemoving,
        }
    }
}
