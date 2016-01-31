using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Logging
{
    public interface IOutputLogger : IBaseLogger
    {
        void Warn(Exception ex, string message);

        void Error(Exception ex, string message);

        void Warn(ExceptionLogInfo exceptionInfo, string message);

        void Error(ExceptionLogInfo exceptionInfo, string message);
    }
}
