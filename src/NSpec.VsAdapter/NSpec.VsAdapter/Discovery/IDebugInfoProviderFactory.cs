using NSpec.VsAdapter.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSpec.VsAdapter.Discovery
{
    public interface IDebugInfoProviderFactory
    {
        IDebugInfoProvider Create(string binaryPath, ICrossDomainLogger logger);
    }
}
