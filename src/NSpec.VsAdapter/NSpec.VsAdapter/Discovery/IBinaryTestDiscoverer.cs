using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Discovery
{
    public interface IBinaryTestDiscoverer
    {
        IEnumerable<NSpecSpecification> Discover(string assemblyPath, IOutputLogger logger, IReplayLogger crossDomainLogger);
    }
}
