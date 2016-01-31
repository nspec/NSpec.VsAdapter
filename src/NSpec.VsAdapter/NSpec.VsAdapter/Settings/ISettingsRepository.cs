using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Settings
{
    public interface ISettingsRepository
    {
        string LogLevel { get; }
    }
}
