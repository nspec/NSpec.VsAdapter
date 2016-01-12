using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.CrossDomain
{
    public class MarshalingProxy<TResult> : MarshalByRefObject, IDisposable
    {
        public override object InitializeLifetimeService()
        {
            // Claim an infinite lease lifetime by returning null here. 
            // To prevent memory leaks as a side effect, instance creators 
            // *must* Dispose() in order to explicitly end the lifetime.

            return null;
        }

        public virtual TResult Execute(Func<TResult> targetOperation) // made virtual to allow test mocking
        {
            return targetOperation();
        }

        // see https://github.com/fixie/fixie/blob/master/src/Fixie/Execution/LongLivedMarshalByRefObject.cs

        public virtual void Dispose() // made virtual to allow test mocking
        {
            RemotingServices.Disconnect(this);
        }
    }
}
