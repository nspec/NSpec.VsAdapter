using NSpec.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSpec.VsAdapter.Core.Discovery.Target
{
    public interface IDiscoveredExampleMapper
    {
        DiscoveredExample FromExample(ExampleBase example);
    }
}
