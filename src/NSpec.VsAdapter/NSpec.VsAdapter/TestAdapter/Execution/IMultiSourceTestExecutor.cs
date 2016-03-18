using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.TestAdapter.Execution
{
    public interface IMultiSourceTestExecutor
    {
        void RunTests(IFrameworkHandle frameworkHandle, IRunContext runContext);

        void CancelRun();
    }
}
