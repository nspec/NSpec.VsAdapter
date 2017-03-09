using Microsoft.VisualStudio.Shell.Interop;

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
