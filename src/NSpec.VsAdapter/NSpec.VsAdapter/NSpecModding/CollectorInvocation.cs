using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSpec.VsAdapter.NSpecModding
{
    public class CollectorInvocation : ICollectorInvocation
    {
        public CollectorInvocation(string assemblyPath)
        {
            this.assemblyPath = assemblyPath;
        }

        public IEnumerable<NSpecSpecification> Collect()
        {
            throw new NotImplementedException();
        }

        readonly string assemblyPath;
    }
}
