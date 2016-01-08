using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter
{
    public class OutputLogger : IOutputLogger
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
            LogException(Warn, ex, message);
        }

        public void Error(Exception ex, string message)
        {
            LogException(Error, ex, message);
        }

        void LogException(LogMethod logMethod, Exception ex, string message)
        {
            if (debugMode)
            {
                logMethod(message);
                logMethod(ex.ToString());
            }
            else
            {
                logMethod(String.Format("{0} [ {1} ]", message, ex.GetType()));
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
