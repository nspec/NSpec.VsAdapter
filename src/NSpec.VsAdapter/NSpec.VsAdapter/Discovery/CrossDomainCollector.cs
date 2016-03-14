using NSpec.VsAdapter.CrossDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSpec.VsAdapter.Discovery
{
    [Serializable]
    public class CrossDomainCollector : 
        CrossDomainRunner<IEnumerable<DiscoveredExample>>, 
        ICrossDomainCollector
    {
        public CrossDomainCollector(
            IAppDomainFactory appDomainFactory,
            IProxyFactory<IEnumerable<DiscoveredExample>> proxyFactory)
            : base(appDomainFactory, proxyFactory) 
        {
        }
    }
}
