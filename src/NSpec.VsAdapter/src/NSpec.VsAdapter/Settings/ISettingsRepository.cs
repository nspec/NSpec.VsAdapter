using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;

namespace NSpec.VsAdapter.Settings
{
    public interface ISettingsRepository
    {
        IAdapterSettings Load(IDiscoveryContext discoveryContext);
    }
}
