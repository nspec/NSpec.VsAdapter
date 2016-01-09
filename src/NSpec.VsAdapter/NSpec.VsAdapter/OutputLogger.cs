using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter
{
    public class OutputLogger : IReplayLogger
    {
        public OutputLogger(IMessageLogger messageLogger, IAdapterInfo adapterInfo)
        {
            this.MessageLogger = messageLogger;

            adapterPrefix = String.Format("{0} {1}", adapterInfo.Name, adapterInfo.Version);
        }

        public IMessageLogger MessageLogger { get; set; } // public getter needed for unit tests

        public void Debug(string message)
        {
            if (debugMode)
            {
                SendOutputMessage(TestMessageLevel.Informational, LevelPrefix.Debug, message);
            }
        }

        public void Info(string message)
        {
            SendOutputMessage(TestMessageLevel.Informational, LevelPrefix.Info, message);
        }

        public void Warn(string message)
        {
            SendOutputMessage(TestMessageLevel.Warning, LevelPrefix.Warn, message);
        }

        public void Error(string message)
        {
            SendOutputMessage(TestMessageLevel.Error, LevelPrefix.Error, message);
        }

        public void Warn(Exception ex, string message)
        {
            LogExceptionInfo(Warn, ex, message);
        }

        public void Error(Exception ex, string message)
        {
            LogExceptionInfo(Error, ex, message);
        }

        public void Warn(ExceptionLogInfo exceptionInfo, string message)
        {
            LogExceptionInfo(Warn, exceptionInfo, message);
        }

        public void Error(ExceptionLogInfo exceptionInfo, string message)
        {
            LogExceptionInfo(Error, exceptionInfo, message);
        }

        void LogExceptionInfo(LogMethod logMethod, ExceptionLogInfo exceptionInfo, string message)
        {
            if (debugMode)
            {
                logMethod(message);
                logMethod(exceptionInfo.Content);
            }
            else
            {
                logMethod(String.Format("{0} [ {1} ]", message, exceptionInfo.Type));
            }
        }

        void SendOutputMessage(TestMessageLevel level, string levelPrefix, string message)
        {
            string outputMessage = String.Format("{0}: {1}{2}", adapterPrefix, levelPrefix, message);

            MessageLogger.SendMessage(level, outputMessage);
        }

        readonly string adapterPrefix;

        readonly bool debugMode = 
#if DEBUG
            true;
#else
            true;
#endif

        delegate void LogMethod(string message);

        static class LevelPrefix
        {
            public const string Debug = "[DEBUG] ";
            public const string Info  = "[INFO]  ";
            public const string Warn  = "[WARN]  ";
            public const string Error = "[ERROR] ";
        }
    }
}
