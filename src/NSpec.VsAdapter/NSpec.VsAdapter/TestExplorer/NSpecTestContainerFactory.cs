using Microsoft.VisualStudio.TestWindow.Extensibility;
using NSpec.VsAdapter.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.TestExplorer
{
    public class NSpecTestContainerFactory : ITestContainerFactory
    {
        public NSpecTestContainerFactory(IFileService fileService)
        {
            this.fileService = fileService;

            debugEngines = new Guid[0];
        }

        public ITestContainer Create(ITestContainerDiscoverer containerDiscoverer, string dllPath)
        {
            return new NSpecTestContainer(containerDiscoverer, dllPath, debugEngines, fileService);
        }

        readonly IFileService fileService;
        readonly Guid[] debugEngines;
    }
}
