using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Settings
{
    public class JsonSettingsRepository : ISettingsRepository
    {
        public JsonSettingsRepository(IFileService fileService)
        {
            settings = noSettings;

            this.fileService = fileService;
        }

        public string BinaryPath 
        { 
            set
            {
                string binaryFolderPAth = Path.GetDirectoryName(value);
                string settingsPath = Path.Combine(binaryFolderPAth, SettingsFileName);

                if (!fileService.Exists(settingsPath))
                {
                    settings = noSettings;
                    return;
                }

                string settingsFileContent;

                try
                {
                    settingsFileContent = fileService.ReadAllText(settingsPath);
                }
                catch (Exception)
                {
                    // TODO log settings problem

                    settings = noSettings;
                    return;
                }

                try
                {
                    settings = JObject.Parse(settingsFileContent);
                }
                catch (Exception)
                {
                    // TODO log settings format problem

                    settings = noSettings;
                    return;
                }
            }
        }

        public string LogLevel
        {
            get 
            {
                const string propertyName = "logLevel";
                const string defaultLogLevel = "info";

                if (settings == noSettings)
                {
                    return defaultLogLevel;
                }

                IDictionary<string, JToken> settingsDictionary = settings;

                if (!settingsDictionary.ContainsKey(propertyName))
                {
                    return defaultLogLevel;
                }

                JToken propertyValue = settingsDictionary[propertyName];

                string logLevel = (string)propertyValue;

                if(String.IsNullOrWhiteSpace(logLevel))
                {
                    return defaultLogLevel;
                }

                return logLevel;
            }
        }

        JObject settings;

        readonly IFileService fileService;

        const JObject noSettings = null;

        public const string SettingsFileName = "nspec-vs.json";
    }
}
