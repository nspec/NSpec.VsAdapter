using NSpec.VsAdapter.CrossDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSpec.VsAdapter.Discovery
{
    [Serializable]
    public class CrossDomainCollector : 
        CrossDomainRunner<IEnumerable<NSpecSpecification>>, 
        ICrossDomainCollector
    {
        public CrossDomainCollector(
            IAppDomainFactory appDomainFactory,
            IMarshalingFactory<IEnumerable<NSpecSpecification>> marshalingFactory)
            : base(appDomainFactory, marshalingFactory) { }
    }
}
