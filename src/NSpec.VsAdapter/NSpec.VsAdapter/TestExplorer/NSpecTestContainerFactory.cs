using Microsoft.VisualStudio.TestWindow.Extensibility;
using NSpec.VsAdapter.Common;
using System;

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
