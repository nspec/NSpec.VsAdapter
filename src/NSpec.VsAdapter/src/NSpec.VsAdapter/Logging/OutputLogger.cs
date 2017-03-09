using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using NSpec.VsAdapter.Settings;
using System;
using System.Collections.Generic;

namespace NSpec.VsAdapter.Logging
{
    public class OutputLogger : IOutputLogger
    {
        public OutputLogger(IMessageLogger messageLogger, IAdapterInfo adapterInfo, IAdapterSettings settings)
        {
            this.MessageLogger = messageLogger;

            adapterPrefix = String.Format("{0} {1}", adapterInfo.Name, adapterInfo.Version);

            var settingsToLogLevelMap = new Dictionary<string, int>()
            {
                { "trace", traceLogLevel },
                { "debug", debugLogLevel },
                { "info", infoLogLevel },
                { "warning", warningLogLevel },
                { "error", errorLogLevel },
            };

            const string defaultSetting = "info";

            string textLogLevel = (settings.LogLevel != null ? settings.LogLevel.ToLower() : defaultSetting);

            minLogLevel = settingsToLogLevelMap.ContainsKey(textLogLevel) ?
                settingsToLogLevelMap[textLogLevel] :
                settingsToLogLevelMap[defaultSetting];
        }

        public IMessageLogger MessageLogger { get; set; } // public getter needed for unit tests

        // core methods taken from https://github.com/osoftware/NSpecTestAdapter/blob/master/NSpec.TestAdapter/TestLogger.cs

        // message

        public void Trace(string message)
        {
            LogMessage(traceLogLevel, LevelPrefix.Trace, TestMessageLevel.Informational, message);
        }

        public void Debug(string message)
        {
            LogMessage(debugLogLevel, LevelPrefix.Debug, TestMessageLevel.Informational, message);
        }

        public void Info(string message)
        {
            LogMessage(infoLogLevel, LevelPrefix.Info, TestMessageLevel.Informational, message);
        }

        public void Warn(string message)
        {
            LogMessage(warningLogLevel, LevelPrefix.Warn, TestMessageLevel.Warning, message);
        }

        public void Error(string message)
        {
            LogMessage(errorLogLevel, LevelPrefix.Error, TestMessageLevel.Error, message);
        }

        // exception and message

        public void Trace(Exception ex, string message)
        {
            LogException(Trace, ex, message);
        }

        public void Debug(Exception ex, string message)
        {
            LogException(Debug, ex, message);
        }

        public void Info(Exception ex, string message)
        {
            LogException(Info, ex, message);
        }

        public void Warn(Exception ex, string message)
        {
            LogException(Warn, ex, message);
        }

        public void Error(Exception ex, string message)
        {
            LogException(Error, ex, message);
        }

        // exception info and message

        public void Trace(ExceptionLogInfo exceptionInfo, string message)
        {
            LogExceptionInfo(Trace, exceptionInfo, message);
        }

        public void Debug(ExceptionLogInfo exceptionInfo, string message)
        {
            LogExceptionInfo(Debug, exceptionInfo, message);
        }

        public void Info(ExceptionLogInfo exceptionInfo, string message)
        {
            LogExceptionInfo(Info, exceptionInfo, message);
        }

        public void Warn(ExceptionLogInfo exceptionInfo, string message)
        {
            LogExceptionInfo(Warn, exceptionInfo, message);
        }

        public void Error(ExceptionLogInfo exceptionInfo, string message)
        {
            LogExceptionInfo(Error, exceptionInfo, message);
        }

        void LogMessage(int logLevel, string levelPrefix, TestMessageLevel testMessageLevel, string message)
        {
            if (logLevel >= minLogLevel)
            {
                SendOutputMessage(testMessageLevel, levelPrefix, message);
            }
        }

        void LogException(LogMethod logMethod, Exception ex, string message)
        {
            var exceptionInfo = new ExceptionLogInfo(ex);

            LogExceptionInfo(logMethod, exceptionInfo, message);
        }

        void LogExceptionInfo(LogMethod logMethod, ExceptionLogInfo exceptionInfo, string message)
        {
            logMethod(String.Format("{0} [ {1} ]", message, exceptionInfo.Type));

            if (debugMode)
            {
                // when in debug mode, log additional exception details
                logMethod(exceptionInfo.Content);
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

        const int traceLogLevel = 1;
        const int debugLogLevel = 2;
        const int infoLogLevel = 3;
        const int warningLogLevel = 4;
        const int errorLogLevel = 5;

        static class LevelPrefix
        {
            public const string Trace = "[TRACE] ";
            public const string Debug = "[DEBUG] ";
            public const string Info  = "[INFO]  ";
            public const string Warn  = "[WARN]  ";
            public const string Error = "[ERROR] ";
        }
    }
}
