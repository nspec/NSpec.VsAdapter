using NSpec.VsAdapter.Core.CrossDomain;
using NSpec.VsAdapter.Core.Execution.Target;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Core.Execution
{
    public class ProxyableTestExecutorFactory : ProxyableFactory<ProxyableTestExecutor, IProxyableTestExecutor>
    {
    }
}
