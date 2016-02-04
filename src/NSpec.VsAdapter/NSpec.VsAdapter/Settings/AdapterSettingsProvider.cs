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
    [SettingsName(AdapterSettings.SettingsName)]
    public class AdapterSettingsProvider : ISettingsProvider
    {
        public AdapterSettingsProvider()
        {
            // initialize default settings, if requested before load
            Settings = new AdapterSettings();

            serializer = new XmlSerializer(typeof(AdapterSettings));
        }

        public AdapterSettings Settings { get; private set; }

        public void Load(XmlReader reader)
        {
            ValidateArg.NotNull(reader, "reader");

            if (reader.Read() && reader.Name == AdapterSettings.SettingsName)
            {
                // store settings locally
                Settings = serializer.Deserialize(reader) as AdapterSettings;
            }
        }

        readonly XmlSerializer serializer;
    }
}
