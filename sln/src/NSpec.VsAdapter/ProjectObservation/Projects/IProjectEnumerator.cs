using System.Collections.Generic;

namespace NSpec.VsAdapter.ProjectObservation.Projects
{
    public interface IProjectEnumerator
    {
        IEnumerable<ProjectInfo> GetLoadedProjects(SolutionInfo solutionInfo);
    }
}
