using NSpec.VsAdapter.Logging;
using System.Collections.Generic;

namespace NSpec.VsAdapter.Core.Discovery
{
    public interface IBinaryTestDiscoverer
    {
        IEnumerable<DiscoveredExample> Discover(string binaryPath, IOutputLogger logger, ICrossDomainLogger crossDomainLogger);
    }
}
