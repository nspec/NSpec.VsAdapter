using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSpec.VsAdapter.ProjectObservation
{
    public class ProjectBuildNotifier : IProjectBuildNotifier, IDisposable
    {
        public IObservable<IEnumerable<ProjectBuildInfo>> BuildStream { get; private set; }
    }
}
