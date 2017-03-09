using Microsoft.VisualStudio.TestWindow.Extensibility;

namespace NSpec.VsAdapter.TestExplorer
{
    public interface ITestContainerFactory
    {
        ITestContainer Create(ITestContainerDiscoverer containerDiscoverer, string dllPath);
    }
}
