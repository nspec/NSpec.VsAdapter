using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSpec.VsAdapter.ProjectObservation.Projects
{
    public interface IProjectConverter
    {
        string ToTestDllPath(ProjectInfo projectInfo);
    }
}
