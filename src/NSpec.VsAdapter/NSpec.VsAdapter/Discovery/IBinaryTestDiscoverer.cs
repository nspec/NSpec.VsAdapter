using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Discovery
{
    public interface IBinaryTestDiscoverer
    {
        IEnumerable<DiscoveredExample> Discover(string binaryPath, IOutputLogger logger, IReplayLogger crossDomainLogger);
    }
}
