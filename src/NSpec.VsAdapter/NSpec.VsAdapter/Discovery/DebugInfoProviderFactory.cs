using NSpec.VsAdapter.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Discovery
{
    public class DebugInfoProviderFactory : IDebugInfoProviderFactory
    {
        public IDebugInfoProvider Create(string binaryPath, ICrossDomainLogger logger)
        {
            return new DebugInfoProvider(binaryPath, logger);
        }
    }
}
