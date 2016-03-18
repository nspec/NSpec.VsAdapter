using NSpec.VsAdapter.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Core.Discovery
{
    public interface IBinaryTestDiscoverer
    {
        IEnumerable<DiscoveredExample> Discover(string binaryPath, IOutputLogger logger, ICrossDomainLogger crossDomainLogger);
    }
}
