using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.ProjectObservation
{
    public class SolutionInfo
    {
        public string DescriptiveName { get; private set; }

        public IVsSolution Solution
        {
            get { return solution; }
            set
            {
                solution = value;

                IVsHierarchy hierarchy = solution as IVsHierarchy;

                string rootName = HierarchyUtils.GetRootName(hierarchy);

                DescriptiveName = rootName;
            }
        }

        IVsSolution solution;
    }
}
