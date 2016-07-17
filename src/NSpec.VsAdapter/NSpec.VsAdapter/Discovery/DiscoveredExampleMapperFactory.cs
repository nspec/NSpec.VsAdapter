using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSpec.VsAdapter.Discovery
{
    public class DiscoveredExampleMapperFactory : IDiscoveredExampleMapperFactory
    {
        public IDiscoveredExampleMapper Create(string binaryPath, IDebugInfoProvider debugInfoProvider)
        {
            return new DiscoveredExampleMapper(binaryPath, debugInfoProvider);
        }
    }
}
