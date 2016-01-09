using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter
{
    public interface IReplayLogger : IBaseLogger
    {
        void Warn(ExceptionLogInfo exceptionInfo, string message);

        void Error(ExceptionLogInfo exceptionInfo, string message);
    }
}
