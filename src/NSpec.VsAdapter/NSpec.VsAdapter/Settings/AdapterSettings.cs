using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace NSpec.VsAdapter.Settings
{
    public class AdapterSettings : TestRunSettings, IAdapterSettings
    {
        public AdapterSettings()
            : base(SettingsName)
        {
            LogLevel = String.Empty;
        }

        public string LogLevel { get; set; }

        public override XmlElement ToXml()
        {
            var stringWriter = new StringWriter();

            serializer.Serialize(stringWriter, this);

            var xml = stringWriter.ToString();

            var document = new XmlDocument();
            
            document.LoadXml(xml);

            return document.DocumentElement;
        }

        public const string SettingsName = "NSpec.VsAdapter.Settings";

        static readonly XmlSerializer serializer = new XmlSerializer(typeof(AdapterSettings));
    }
}
