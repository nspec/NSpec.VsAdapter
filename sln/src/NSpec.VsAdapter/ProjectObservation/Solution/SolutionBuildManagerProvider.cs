using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace NSpec.VsAdapter.ProjectObservation.Solution
{
    public class SolutionBuildManagerProvider : ISolutionBuildManagerProvider
    {
        public IVsSolutionBuildManager2 Provide()
        {
            // TODO check result and manage failure

            IVsSolutionBuildManager2 solutionBuildManager = 
                ServiceProvider.GlobalProvider.GetService(typeof(SVsSolutionBuildManager)) as IVsSolutionBuildManager2;

            return solutionBuildManager;
        }
    }
}
