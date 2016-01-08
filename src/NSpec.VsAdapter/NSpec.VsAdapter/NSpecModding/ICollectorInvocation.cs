using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSpec.VsAdapter.NSpecModding
{
    public interface ICollectorInvocation
    {
        NSpecSpecification[] Collect();
    }
}
