using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSpec.VsAdapter.NSpecModding
{
    public interface ICrossDomainCollector : ICrossDomainRunner<ICollectorInvocation, IEnumerable<NSpecSpecification>>
    {
    }
}
