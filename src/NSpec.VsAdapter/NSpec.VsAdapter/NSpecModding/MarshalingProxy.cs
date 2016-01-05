using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.NSpecModding
{
    public class MarshalingProxy<TInvocation, TResult> : MarshalByRefObject
    {
        public override object InitializeLifetimeService()
        {
            return null;
        }

        public virtual TResult Execute(TInvocation invocation, Func<TInvocation, TResult> targetOperation)
        {
            return targetOperation(invocation);
        }
    }
}
