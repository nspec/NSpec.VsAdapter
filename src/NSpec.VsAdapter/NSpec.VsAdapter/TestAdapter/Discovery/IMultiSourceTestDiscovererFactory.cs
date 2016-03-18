using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.TestAdapter.Discovery
{
    public interface IMultiSourceTestDiscovererFactory
    {
        IMultiSourceTestDiscoverer Create(IEnumerable<string> sources);
    }
}
