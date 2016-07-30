using System;

namespace NSpec.VsAdapter.ProjectObservation.Projects
{
    public interface IProjectBuildNotifier
    {
        IObservable<ProjectInfo> BuildStream { get; }
    }
}
