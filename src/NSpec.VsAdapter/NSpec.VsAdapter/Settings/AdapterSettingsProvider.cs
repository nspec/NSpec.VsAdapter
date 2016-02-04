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
        public AdapterSettingsProvider()
        {
            // initialize default settings, if requested before load
            Settings = new AdapterSettings();

            serializer = new XmlSerializer(typeof(AdapterSettings));
        }

        public AdapterSettings Settings { get; private set; }

        public void Load(XmlReader reader)
        {
            // TODO test that AdapterSettingsProvider.Load does not throw when deserialize fails

            ValidateArg.NotNull(reader, "reader");

            if (reader.Read() && reader.Name == AdapterSettings.RunSettingsXmlNode)
            {
                // store settings locally
                Settings = serializer.Deserialize(reader) as AdapterSettings;
            }
        }

        readonly XmlSerializer serializer;
    }
}
