using NSpec.VsAdapter.CrossDomain;
using NSpec.VsAdapter.Execution.Target;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Execution
{
    public class ProxyableTestExecutorFactory : ProxyableFactory<ProxyableTestExecutor, IProxyableTestExecutor>
    {
    }
}
