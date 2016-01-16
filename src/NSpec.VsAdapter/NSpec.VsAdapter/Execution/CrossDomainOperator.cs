using NSpec.VsAdapter.CrossDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Execution
{
    public class CrossDomainOperator :
        CrossDomainRunner<int>, 
        ICrossDomainOperator
    {
        public CrossDomainOperator(
            IAppDomainFactory appDomainFactory,
            IMarshalingFactory<int> marshalingFactory)
            : base(appDomainFactory, marshalingFactory)
        {
        }
    }
}
