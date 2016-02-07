using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace NSpec.VsAdapter.Settings
{
    [SettingsName(AdapterSettings.RunSettingsXmlNode)]
    public class AdapterSettingsProvider : ISettingsProvider
    {
        // Visual Studio test infrastructure requires a default constructor
        public AdapterSettingsProvider()
            : this(new XmlSerializer(typeof(AdapterSettings)))
        {
        }

        // Unit tests need a constructor with injected dependencies
        public AdapterSettingsProvider(XmlSerializer serializer)
        {
            // initialize default settings, if requested before load
            Settings = new AdapterSettings();

            this.serializer = serializer;
        }

        public AdapterSettings Settings { get; private set; }

        public void Load(XmlReader reader)
        {
            if (reader == null)
            {
                Settings = new AdapterSettings();
                return;
            }

            try
            {
                if (reader.Read() && reader.Name == AdapterSettings.RunSettingsXmlNode)
                {
                    // store settings locally
                    Settings = serializer.Deserialize(reader) as AdapterSettings;
                }
                else
                {
                    Settings = new AdapterSettings();
                }
            }
            catch (Exception)
            {
                // Swallow exception, probably cannot even log at this time

                Settings = new AdapterSettings();
            }
        }

        readonly XmlSerializer serializer;
    }
}
