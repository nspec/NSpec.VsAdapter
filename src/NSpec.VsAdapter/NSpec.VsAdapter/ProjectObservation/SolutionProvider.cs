using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.ProjectObservation
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
