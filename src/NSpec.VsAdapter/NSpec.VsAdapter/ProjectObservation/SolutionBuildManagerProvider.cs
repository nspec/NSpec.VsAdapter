using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.ProjectObservation
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
