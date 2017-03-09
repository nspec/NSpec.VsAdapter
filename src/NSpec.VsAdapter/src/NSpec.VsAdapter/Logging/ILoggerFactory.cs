using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using NSpec.VsAdapter.Settings;

namespace NSpec.VsAdapter.Logging
{
    public interface ILoggerFactory
    {
        IOutputLogger CreateOutput(IMessageLogger messageLogger, IAdapterSettings settings);
    }
}
