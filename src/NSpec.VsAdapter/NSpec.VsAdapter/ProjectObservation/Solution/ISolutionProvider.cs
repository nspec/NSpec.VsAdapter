using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.ProjectObservation.Solution
{
    public interface ISolutionProvider
    {
        IVsSolution Provide();
    }
}
