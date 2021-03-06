﻿using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using System;

namespace NSpec.VsAdapter.Settings
{
    public class SettingsRepository : ISettingsRepository
    {
        public IAdapterSettings Load(IDiscoveryContext discoveryContext)
        {
            IAdapterSettingsProvider settingsProvider;

            try
            {
                settingsProvider = discoveryContext.RunSettings.GetSettings(AdapterSettings.RunSettingsXmlNode) as IAdapterSettingsProvider;
            }
            catch (Exception)
            {
                // Swallow exception, probably cannot even log at this time

                settingsProvider = null;
            }

            var settings = (settingsProvider != null ? settingsProvider.Settings : new AdapterSettings());

            return settings;
        }
    }
}
