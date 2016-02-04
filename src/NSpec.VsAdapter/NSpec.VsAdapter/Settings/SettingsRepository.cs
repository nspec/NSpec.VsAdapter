using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Settings
{
    public class SettingsRepository : ISettingsRepository
    {
        public IAdapterSettings Load(IDiscoveryContext discoveryContext)
        {
            // TODO test that SettingsRepository.Load does not throw when GetSettings throws

            var settingsProvider = discoveryContext.RunSettings.GetSettings(AdapterSettings.RunSettingsXmlNode) as AdapterSettingsProvider;

            var settings = (settingsProvider != null ?  settingsProvider.Settings : new AdapterSettings());

            return settings;
        }
    }
}
