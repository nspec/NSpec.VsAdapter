using Microsoft.VisualStudio.TestWindow.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.TestExplorer
{
    public interface ITestContainerFactory
    {
        ITestContainer Create(ITestContainerDiscoverer containerDiscoverer, string dllPath);
    }
}
