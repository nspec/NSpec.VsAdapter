using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.NSpecModding
{
    public interface ICrossDomainTestDiscoverer
    {
        IEnumerable<NSpecSpecification> Discover(string assemblyPath, IMessageLogger logger);
    }
}
