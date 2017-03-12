using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigSampleSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            var systemWithSettings = new SystemWithSettings();

            int appSettingsValue = systemWithSettings.AppSettingsProperty;

            Console.WriteLine(String.Format("App settings value is: {0}.", appSettingsValue));
        }
    }
}
