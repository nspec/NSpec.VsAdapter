using Microsoft.VisualStudio.Shell.Interop;

namespace NSpec.VsAdapter.ProjectObservation.Solution
{
    public interface ISolutionBuildManagerProvider
    {
        IVsSolutionBuildManager2 Provide();
    }
}
