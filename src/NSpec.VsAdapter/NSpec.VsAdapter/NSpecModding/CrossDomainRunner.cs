using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSpec.VsAdapter.NSpecModding
{
    public class CrossDomainRunner : ICrossDomainRunner
    {
        public IEnumerable<NSpecSpecification> Run(
            string assemblyPath, 
            ICollectorInvocation invocation,
            Func<ICollectorInvocation, IEnumerable<NSpecSpecification>> outputSelector)
        {
            throw new NotImplementedException();
        }
    }
}
