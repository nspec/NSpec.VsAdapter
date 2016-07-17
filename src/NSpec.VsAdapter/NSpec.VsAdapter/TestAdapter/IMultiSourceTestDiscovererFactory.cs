using System.Collections.Generic;

namespace NSpec.VsAdapter.TestAdapter
{
    public interface IMultiSourceTestDiscovererFactory
    {
        IMultiSourceTestDiscoverer Create(IEnumerable<string> sources);
    }
}
