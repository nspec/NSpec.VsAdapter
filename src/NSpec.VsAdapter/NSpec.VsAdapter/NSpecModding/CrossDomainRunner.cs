using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSpec.VsAdapter.NSpecModding
{
    [Serializable]
    public class CrossDomainRunner : 
        NspecDomainRunner<ICollectorInvocation, IEnumerable<NSpecSpecification>>, 
        ICrossDomainRunner
    {
        public CrossDomainRunner(IAppDomainFactory appDomainFactory) : base(appDomainFactory) {}
    }
}
