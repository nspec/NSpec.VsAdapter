using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;

namespace NSpec.VsAdapter.ProjectObservation.Projects
{
    public class ProjectWrapperFactory : IProjectWrapperFactory
    {
        public IProjectWrapper Create(IVsHierarchy projectHierarchy)
        {
            var autoProject = GetAutomationProject(projectHierarchy);

            if (autoProject == null)
            {
                return null;
            }

            return new ProjectWrapper(autoProject);
        }

        static Project GetAutomationProject(IVsHierarchy projectHierarchy)
        {
            const Project noAutomationProject = null;

            object outputBuffer;
            int result;

            result = projectHierarchy.GetProperty(
                (uint)VSConstants.VSITEMID.Root, (int)__VSHPROPID.VSHPROPID_ExtObject, out outputBuffer);

            if (ErrorHandler.Failed(result) || outputBuffer == null)
            {
                return noAutomationProject;
            }

            var autoProject = outputBuffer as Project;

            if (autoProject == null)
            {
                return noAutomationProject;
            }

            return autoProject;
        }
    }
}
