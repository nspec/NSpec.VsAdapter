using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter
{
    public class ProjectBuildInfo
    {
        public IVsHierarchy Hierarchy { get; set; }

        /* Available from VSX:
         * 
        IVsHierarchy pHierProj,
        IVsCfg pCfgProj,
        IVsCfg pCfgSln,
        uint dwAction,
        int fSuccess,
        int fCancel
         */
    }
}
