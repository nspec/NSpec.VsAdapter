using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using NSpec.VsAdapter.Settings;

namespace NSpec.VsAdapter.Logging
{
    public class LoggerFactory : ILoggerFactory
    {
        public LoggerFactory(IAdapterInfo adapterInfo)
        {
            this.adapterInfo = adapterInfo;
        }

        public IOutputLogger CreateOutput(IMessageLogger messageLogger, IAdapterSettings settings)
        {
            return new OutputLogger(messageLogger, adapterInfo, settings);
        }

        readonly IAdapterInfo adapterInfo;
    }
}
