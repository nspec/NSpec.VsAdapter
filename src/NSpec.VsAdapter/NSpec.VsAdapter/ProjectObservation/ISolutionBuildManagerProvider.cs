using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSpec.VsAdapter.ProjectObservation
{
    public interface ISolutionBuildManagerProvider
    {
        IVsSolutionBuildManager2 Provide();
    }
}
