using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSpec.VsAdapter.Discovery
{
    public interface IDiscoveredExampleMapperFactory
    {
        IDiscoveredExampleMapper Create(string binaryPath, IDebugInfoProvider debugInfoProvider);
    }
}
