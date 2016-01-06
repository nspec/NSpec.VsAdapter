using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSpec.VsAdapter.NSpecModding
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

        public override IEnumerable<NSpecSpecification> Run(
            string assemblyPath, 
            Func<IEnumerable<NSpecSpecification>> targetOperation)
        {
            var specifications = base.Run(assemblyPath, targetOperation);

            return (specifications != null ? specifications : new NSpecSpecification[0]);
        }
    }
}
