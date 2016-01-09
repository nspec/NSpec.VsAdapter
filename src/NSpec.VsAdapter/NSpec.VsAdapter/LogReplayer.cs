using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter
{
    class LogReplayer
    {
        public LogReplayer(IReplayLogger logger)
        {
            logLevelToMessageActionMap = new Dictionary<LogMessageLevel, MessageLogAction>()
            {
                { LogMessageLevel.Debug, logger.Debug },
                { LogMessageLevel.Info, logger.Info },
                { LogMessageLevel.Warn, logger.Warn },
                { LogMessageLevel.Error, logger.Error },
            };

            logLevelToExceptionInfoActionMap = new Dictionary<LogMessageLevel, ExceptionInfoLogAction>()
            {
                { LogMessageLevel.Warn, logger.Warn },
                { LogMessageLevel.Error, logger.Error },
            };
        }

        public void Replay(LogRecorder logRecorder)
        {
            var stream = logRecorder.Stream;

            stream.Position = 0;

            var recordedEvents = Serializer.Deserialize<List<RecordedLogMessage>>(stream);

            recordedEvents.Do(ReplayEvent);
        }

        void ReplayEvent(RecordedLogMessage recordedEvent)
        {
            var messageLogAction = logLevelToMessageActionMap[recordedEvent.LogLevel];

            messageLogAction(recordedEvent.Message);
        }

        void ReplayEvent(RecordedLogException recordedEvent)
        {
            var exceptionLogAction = logLevelToExceptionInfoActionMap[recordedEvent.LogLevel];

            exceptionLogAction(recordedEvent.ExceptionInfo, recordedEvent.Message);
        }

        readonly Dictionary<LogMessageLevel, MessageLogAction> logLevelToMessageActionMap;
        readonly Dictionary<LogMessageLevel, ExceptionInfoLogAction> logLevelToExceptionInfoActionMap;

        delegate void MessageLogAction(string message);
        delegate void ExceptionInfoLogAction(ExceptionLogInfo exceptionInfo, string message);
    }
}
