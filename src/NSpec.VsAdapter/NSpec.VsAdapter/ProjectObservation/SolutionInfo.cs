using Microsoft.VisualStudio.Shell.Interop;

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
