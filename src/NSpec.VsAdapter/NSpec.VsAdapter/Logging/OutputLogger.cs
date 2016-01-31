using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using NSpec.VsAdapter.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Logging
{
    public class OutputLogger : IOutputLogger, IReplayLogger
    {
        public OutputLogger(IMessageLogger messageLogger, IAdapterInfo adapterInfo, ISettingsRepository settings)
        {
            this.MessageLogger = messageLogger;

            adapterPrefix = String.Format("{0} {1}", adapterInfo.Name, adapterInfo.Version);

            var settingsToLogLevelMap = new Dictionary<string, int>()
            {
                { "debug", debugLogLevel },
                { "info", infoLogLevel },
                { "warning", warningLogLevel },
                { "error", errorLogLevel },
            };

            const string defaultSetting = "info";

            string textLogLevel = (settings.LogLevel != null ? settings.LogLevel : defaultSetting);

            textLogLevel = textLogLevel.ToLower();

            minLogLevel = settingsToLogLevelMap.ContainsKey(textLogLevel) ?
                settingsToLogLevelMap[textLogLevel] :
                settingsToLogLevelMap[defaultSetting];
        }

        public IMessageLogger MessageLogger { get; set; } // public getter needed for unit tests

        // core methods taken from https://github.com/osoftware/NSpecTestAdapter/blob/master/NSpec.TestAdapter/TestLogger.cs

        public void Debug(string message)
        {
            if (debugLogLevel >= minLogLevel)
            {
                SendOutputMessage(TestMessageLevel.Informational, LevelPrefix.Debug, message);
            }
        }

        public void Info(string message)
        {
            if (infoLogLevel >= minLogLevel)
            {
                SendOutputMessage(TestMessageLevel.Informational, LevelPrefix.Info, message);
            }
        }

        public void Warn(string message)
        {
            if (warningLogLevel >= minLogLevel)
            {
                SendOutputMessage(TestMessageLevel.Warning, LevelPrefix.Warn, message);
            }
        }

        public void Error(string message)
        {
            if (errorLogLevel >= minLogLevel)
            {
                SendOutputMessage(TestMessageLevel.Error, LevelPrefix.Error, message);
            }
        }

        public void Warn(Exception ex, string message)
        {
            var exceptionInfo = new ExceptionLogInfo(ex);

            LogExceptionInfo(Warn, exceptionInfo, message);
        }

        public void Error(Exception ex, string message)
        {
            var exceptionInfo = new ExceptionLogInfo(ex);

            LogExceptionInfo(Error, exceptionInfo, message);
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
        readonly int minLogLevel;

        readonly bool debugMode = 
#if DEBUG
            true;
#else
            false;
#endif

        delegate void LogMethod(string message);

        const int debugLogLevel = 1;
        const int infoLogLevel = 2;
        const int warningLogLevel = 3;
        const int errorLogLevel = 4;

        static class LevelPrefix
        {
            public const string Debug = "[DEBUG] ";
            public const string Info  = "[INFO]  ";
            public const string Warn  = "[WARN]  ";
            public const string Error = "[ERROR] ";
        }
    }
}
