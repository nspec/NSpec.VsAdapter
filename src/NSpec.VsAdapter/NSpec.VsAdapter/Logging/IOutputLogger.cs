using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Logging
{
    public interface IOutputLogger : IBaseLogger
    {
        // exception and message

        void Trace(Exception ex, string message);

        void Debug(Exception ex, string message);

        void Info(Exception ex, string message);

        void Warn(Exception ex, string message);

        void Error(Exception ex, string message);

        // exception info and message

        void Trace(ExceptionLogInfo exceptionInfo, string message);

        void Debug(ExceptionLogInfo exceptionInfo, string message);

        void Info(ExceptionLogInfo exceptionInfo, string message);

        void Warn(ExceptionLogInfo exceptionInfo, string message);

        void Error(ExceptionLogInfo exceptionInfo, string message);
    }
}
