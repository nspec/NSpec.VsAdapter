using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.NSpecModding
{
    public interface ICrossDomainTestDiscoverer
    {
        IEnumerable<NSpecSpecification> Discover(string assemblyPath, IOutputLogger logger, IReplayLogger crossDomainLogger);
    }
}
