using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using NSpec.VsAdapter.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Logging
{
    public class LoggerFactory : ILoggerFactory
    {
        public LoggerFactory(IAdapterInfo adapterInfo, ISettingsRepository settings)
        {
            this.adapterInfo = adapterInfo;
            this.settings = settings;
        }

        public IOutputLogger CreateOutput(IMessageLogger messageLogger)
        {
            return new OutputLogger(messageLogger, adapterInfo, settings);
        }

        readonly IAdapterInfo adapterInfo;
        readonly ISettingsRepository settings;
    }
}
