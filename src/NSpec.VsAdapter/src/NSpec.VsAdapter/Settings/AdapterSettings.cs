using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace NSpec.VsAdapter.Settings
{
    [XmlRoot(AdapterSettings.RunSettingsXmlNode)]
    public class AdapterSettings : TestRunSettings, IAdapterSettings
    {
        public AdapterSettings()
            : base(RunSettingsXmlNode)
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

        static readonly XmlSerializer serializer = new XmlSerializer(typeof(AdapterSettings));

        public const string RunSettingsXmlNode = "NSpec.VsAdapter";
    }
}
