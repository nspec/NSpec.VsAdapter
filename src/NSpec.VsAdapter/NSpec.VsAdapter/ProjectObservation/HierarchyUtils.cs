using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.ProjectObservation
{
    static class HierarchyUtils
    {
        public static string GetRootName(IVsHierarchy hierarchy)
        {
            string noNameFound = String.Empty;

            if (hierarchy == null)
            {
                return noNameFound;
            }

            object outputBuffer;
            int result;

            result = hierarchy.GetProperty(
                (uint)VSConstants.VSITEMID.Root, (int)__VSHPROPID.VSHPROPID_Name, out outputBuffer);

            if (ErrorHandler.Failed(result) || outputBuffer == null)
            {
                return noNameFound;
            }

            string name = outputBuffer as string;

            return (name == null ? noNameFound : name);
        }
    }
}
