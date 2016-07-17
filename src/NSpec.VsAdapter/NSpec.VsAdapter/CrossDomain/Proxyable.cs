using System;
using System.Runtime.Remoting;

namespace NSpec.VsAdapter.CrossDomain
{
    public abstract class Proxyable : MarshalByRefObject, IDisposable
    {
        public override object InitializeLifetimeService()
        {
            // Claim an infinite lease lifetime by returning null here. 
            // To prevent memory leaks as a side effect, instance creators 
            // *must* Dispose() in order to explicitly end the lifetime.

            return null;
        }

        // see https://github.com/fixie/fixie/blob/master/src/Fixie/Execution/LongLivedMarshalByRefObject.cs

        public virtual void Dispose() // made virtual to allow test mocking
        {
            RemotingServices.Disconnect(this);
        }
    }
}