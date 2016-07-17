using Microsoft.VisualStudio.Shell.Interop;

namespace NSpec.VsAdapter.ProjectObservation.Projects
{
    public interface IProjectWrapperFactory
    {
        IProjectWrapper Create(IVsHierarchy projectHierarchy);
    }
}
