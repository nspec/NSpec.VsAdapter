using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Logging
{
    public class CrossDomainLogger : MarshalByRefObject, ICrossDomainLogger, IDisposable
    {
        public CrossDomainLogger(IOutputLogger outputLogger)
        {
            this.outputLogger = outputLogger;
        }

        // MarshalByRefObject

        public override object InitializeLifetimeService()
        {
            // Claim an infinite lease lifetime by returning null here. 
            // To prevent memory leaks as a side effect, instance creators 
            // *must* Dispose() in order to explicitly end the lifetime.

            return null;
        }

        // IDisposable

        public virtual void Dispose()
        {
            RemotingServices.Disconnect(this);
        }

        // ICrossDomainLogger

        public void Debug(string message)
        {
            outputLogger.Debug(message);
        }

        public void Info(string message)
        {
            outputLogger.Info(message);
        }

        public void Warn(string message)
        {
            outputLogger.Warn(message);
        }

        public void Error(string message)
        {
            outputLogger.Error(message);
        }

        public void Warn(ExceptionLogInfo exceptionInfo, string message)
        {
            outputLogger.Warn(exceptionInfo, message);
        }

        public void Error(ExceptionLogInfo exceptionInfo, string message)
        {
            outputLogger.Error(exceptionInfo, message);
        }

        readonly IOutputLogger outputLogger;
    }
}
