using NSpec.VsAdapter.CrossDomain;
using NSpec.VsAdapter.Discovery.Target;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Discovery
{
    public class ProxyableTestDiscovererFactory : ProxyableFactory<ProxyableTestDiscoverer, IProxyableTestDiscoverer>
    {
    }
}
