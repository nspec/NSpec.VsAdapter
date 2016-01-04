using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.ProjectObservation.Projects
{
    public class ProjectEnumerator : IProjectEnumerator
    {
        // mostly copied from https://github.com/Microsoft/VSSDK-Extensibility-Samples/blob/8d2b8fdbb6f2ba1c83f403eefab49cad3710b650/Code_Sweep/C%23/VsPackage/ProjectUtilities.cs

        public IEnumerable<ProjectInfo> GetLoadedProjects(SolutionInfo solutionInfo)
        {
            IVsSolution solutionHierarchy = solutionInfo.Solution;

            IEnumerable<IVsHierarchy> hierarchies = Enumerate(solutionHierarchy, __VSENUMPROJFLAGS.EPF_LOADEDINSOLUTION);

            var projectInfos = hierarchies.Select(h => new ProjectInfo() { Hierarchy = h });

            return projectInfos;
        }

        static IEnumerable<IVsHierarchy> Enumerate(IVsSolution solution, __VSENUMPROJFLAGS enumFlags)
        {
            var enumOnlyThisType = Guid.Empty;

            IEnumHierarchies hierarchyEnumerator;

            var result = solution.GetProjectEnum((uint)enumFlags, ref enumOnlyThisType, out hierarchyEnumerator);

            if (ErrorHandler.Succeeded(result) && hierarchyEnumerator != null)
            {
                const int bufferSize = 1;

                var hierarchies = new IVsHierarchy[bufferSize];

                uint nrFetched = 0;

                while (hierarchyEnumerator.Next(bufferSize, hierarchies, out nrFetched) == VSConstants.S_OK)
                {
                    yield return hierarchies[0];
                }
            }
        }
    }
}
