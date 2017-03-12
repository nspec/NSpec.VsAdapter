using Microsoft.VisualStudio.Shell.Interop;

namespace NSpec.VsAdapter.ProjectObservation.Solution
{
    public interface ISolutionProvider
    {
        IVsSolution Provide();
    }
}
