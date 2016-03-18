using NSpec.VsAdapter.Core.CrossDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Logging
{
    public class CrossDomainLogger : Proxyable, ICrossDomainLogger
    {
        public CrossDomainLogger(IOutputLogger outputLogger)
        {
            this.outputLogger = outputLogger;
        }

        // ICrossDomainLogger

        // message

        public void Trace(string message)
        {
            outputLogger.Trace(message);
        }

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

        // exception info and message

        public void Trace(ExceptionLogInfo exceptionInfo, string message)
        {
            outputLogger.Trace(exceptionInfo, message);
        }

        public void Debug(ExceptionLogInfo exceptionInfo, string message)
        {
            outputLogger.Debug(exceptionInfo, message);
        }

        public void Info(ExceptionLogInfo exceptionInfo, string message)
        {
            outputLogger.Info(exceptionInfo, message);
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
