using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace NSpec.VsAdapter.ProjectObservation.Solution
{
    public class SolutionProvider : ISolutionProvider
    {
        public IVsSolution Provide()
        {
            // TODO check result and manage failure

            IVsSolution solution = ServiceProvider.GlobalProvider.GetService(typeof(SVsSolution)) as IVsSolution;

            return solution;
        }
    }
}
