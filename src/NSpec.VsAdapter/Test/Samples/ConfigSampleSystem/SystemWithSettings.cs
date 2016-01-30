using System;
using System.Configuration;

namespace ConfigSampleSystem
{
    public class SystemWithSettings
    {
        public int AppSettingsProperty
        {
            get
            {
                string somePropertyText = ConfigurationManager.AppSettings["someProperty"];

                if (String.IsNullOrWhiteSpace(somePropertyText))
                {
                    throw new InvalidOperationException("Setting cannot be null, empty, nor whitespace");
                }

                int someProperty;

                bool success = Int32.TryParse(somePropertyText, out someProperty);

                if (!success)
                {
                    throw new InvalidOperationException("Setting must represent an integer number");
                }

                return someProperty;
            }
        }
    }
}
