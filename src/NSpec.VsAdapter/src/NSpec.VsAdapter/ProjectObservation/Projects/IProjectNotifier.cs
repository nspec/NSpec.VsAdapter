using System;
using System.Collections.Generic;

namespace NSpec.VsAdapter.ProjectObservation.Projects
{
    public interface IProjectNotifier
    {
        IObservable<IEnumerable<ProjectInfo>> ProjectStream { get; }
    }
}
