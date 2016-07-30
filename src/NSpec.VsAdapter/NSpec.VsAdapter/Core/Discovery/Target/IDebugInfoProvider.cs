using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace NSpec.VsAdapter.Core.Discovery.Target
{
    public interface IDebugInfoProvider
    {
        DiaNavigationData GetNavigationData(string declaringClassName, string methodName);
    }
}
