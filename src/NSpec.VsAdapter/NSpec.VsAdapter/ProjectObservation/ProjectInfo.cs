using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.ProjectObservation
{
    public class ProjectInfo
    {
        public string DescriptiveName { get; private set; }

        public IVsHierarchy Hierarchy 
        {
            get { return hierarchy; }
            set
            {
                hierarchy = value;

                string rootName = HierarchyUtils.GetRootName(hierarchy);

                DescriptiveName = rootName;
            }
        }

        IVsHierarchy hierarchy;
    }
}
