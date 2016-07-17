using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;

namespace NSpec.VsAdapter.Settings
{
    public interface IAdapterSettingsProvider : ISettingsProvider
    {
        AdapterSettings Settings { get; }
    }
}
