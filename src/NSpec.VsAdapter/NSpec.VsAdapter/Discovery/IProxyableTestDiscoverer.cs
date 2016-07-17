using NSpec.VsAdapter.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Discovery
{
    public interface IProxyableTestDiscoverer : IDisposable
    {
        DiscoveredExample[] Discover(string binaryPath, ICrossDomainLogger logger);
    }
}
