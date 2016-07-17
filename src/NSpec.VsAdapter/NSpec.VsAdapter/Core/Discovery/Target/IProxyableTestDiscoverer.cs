using NSpec.VsAdapter.Logging;
using System;

namespace NSpec.VsAdapter.Core.Discovery.Target
{
    public interface IProxyableTestDiscoverer : IDisposable
    {
        DiscoveredExample[] Discover(string binaryPath, ICrossDomainLogger logger);
    }
}
