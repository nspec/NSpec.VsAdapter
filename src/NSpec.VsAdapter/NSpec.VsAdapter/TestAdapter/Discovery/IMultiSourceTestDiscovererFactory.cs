using System.Collections.Generic;

namespace NSpec.VsAdapter.TestAdapter.Discovery
{
    public interface IMultiSourceTestDiscovererFactory
    {
        IMultiSourceTestDiscoverer Create(IEnumerable<string> sources);
    }
}
