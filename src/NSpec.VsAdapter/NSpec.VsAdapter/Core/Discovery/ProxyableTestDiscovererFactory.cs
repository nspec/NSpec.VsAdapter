using NSpec.VsAdapter.Core.CrossDomain;
using NSpec.VsAdapter.Core.Discovery.Target;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Core.Discovery
{
    public class ProxyableTestDiscovererFactory : ProxyableFactory<ProxyableTestDiscoverer, IProxyableTestDiscoverer>
    {
    }
}
