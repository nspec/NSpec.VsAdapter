using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter
{
    public class CrossDomainTestDiscoverer : ICrossDomainTestDiscoverer
    {
        public IEnumerable<NSpecSpecification> Discover(string assemblyPath)
        {
            throw new NotImplementedException();
        }
    }
}
