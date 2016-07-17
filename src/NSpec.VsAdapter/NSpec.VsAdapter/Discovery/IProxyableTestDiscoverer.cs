using NSpec.VsAdapter.Logging;
using System;

namespace NSpec.VsAdapter.Discovery
{
    public interface IProxyableTestDiscoverer : IDisposable
    {
        DiscoveredExample[] Discover(string binaryPath, ICrossDomainLogger logger);
    }
}
