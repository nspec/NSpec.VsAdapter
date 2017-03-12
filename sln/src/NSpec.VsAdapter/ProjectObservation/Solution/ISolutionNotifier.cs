using System;
using System.Reactive;

namespace NSpec.VsAdapter.ProjectObservation.Solution
{
    public interface ISolutionNotifier
    {
        IObservable<SolutionInfo> SolutionOpenedStream { get; }

        IObservable<Unit> SolutionClosingStream { get; }

        IObservable<ProjectInfo> ProjectAddedStream { get; }

        IObservable<ProjectInfo> ProjectRemovingtream { get; }
    }
}
