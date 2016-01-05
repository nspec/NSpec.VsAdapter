using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSpec.VsAdapter.NSpecModding
{
    [Serializable]
    public class CrossDomainCollector : 
        CrossDomainRunner<ICollectorInvocation, IEnumerable<NSpecSpecification>>, 
        ICrossDomainCollector
    {
        public CrossDomainCollector(
            IAppDomainFactory appDomainFactory,
            IMarshalingFactory<ICollectorInvocation, IEnumerable<NSpecSpecification>> marshalingFactory)
            : base(appDomainFactory, marshalingFactory) { }

        public override IEnumerable<NSpecSpecification> Run(string assemblyPath, ICollectorInvocation invocation, Func<ICollectorInvocation, IEnumerable<NSpecSpecification>> outputSelector)
        {
            var specifications = base.Run(assemblyPath, invocation, outputSelector);

            return (specifications != null ? specifications : new NSpecSpecification[0]);
        }
    }
}
