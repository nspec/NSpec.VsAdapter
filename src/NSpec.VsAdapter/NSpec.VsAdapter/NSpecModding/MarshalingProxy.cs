using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.NSpecModding
{
    public class MarshalingProxy<TResult> : MarshalByRefObject
    {
        public override object InitializeLifetimeService()
        {
            return null;
        }

        public virtual TResult Execute(Func<TResult> targetOperation)
        {
            return targetOperation();
        }
    }
}
