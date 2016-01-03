using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.ProjectObservation
{
    public interface ISolutionNotifier
    {
        IObservable<SolutionInfo> SolutionOpenedStream { get; }

        IObservable<Unit> SolutionClosingStream { get; }

        IObservable<ProjectInfo> ProjectAddedStream { get; }

        IObservable<ProjectInfo> ProjectRemovingtream { get; }
    }
}
