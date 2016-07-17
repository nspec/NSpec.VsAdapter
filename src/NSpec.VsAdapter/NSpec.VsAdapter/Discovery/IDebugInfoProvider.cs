using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace NSpec.VsAdapter.Discovery
{
    public interface IDebugInfoProvider
    {
        DiaNavigationData GetNavigationData(string declaringClassName, string methodName);
    }
}
