using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Settings
{
    public class JsonSettingsRepository : ISettingsRepository
    {
        public string LogLevel // TODO implement
        {
            get { return "Info"; }
        }
    }
}
