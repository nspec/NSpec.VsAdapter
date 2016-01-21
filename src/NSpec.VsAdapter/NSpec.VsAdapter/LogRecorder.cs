using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter
{
    [Serializable]
    public class LogRecorder : ISerializableLogger, IDisposable
    {
        public LogRecorder()
        {
            this.stream = new MemoryStream();
            this.events = new List<RecordedLogMessage>();
        }

        public void Dispose()
        {
            stream.Dispose();
        }

        public void Debug(string message)
        {
            RecordMessage(LogMessageLevel.Debug, message);
        }

        public void Info(string message)
        {
            RecordMessage(LogMessageLevel.Info, message);
        }

        public void Warn(string message)
        {
            RecordMessage(LogMessageLevel.Warn, message);
        }

        public void Warn(Exception ex, string message)
        {
            RecordException(LogMessageLevel.Warn, message, ex);
        }

        public void Error(string message)
        {
            RecordMessage(LogMessageLevel.Error, message);
        }

        public void Error(Exception ex, string message)
        {
            RecordException(LogMessageLevel.Error, message, ex);
        }

        public void Flush()
        {
            // TODO Extract (de)serialization as a dependency

            Serializer.Serialize(stream, events);

            events.Clear();

            stream.Flush();
        }

        public MemoryStream Stream
        {
            get 
            {
                stream.Flush();

                return stream; 
            }
        }

        void RecordMessage(LogMessageLevel level, string message)
        {
            var recordedMessage = new RecordedLogMessage(level, message);
            
            events.Add(recordedMessage);
        }

        void RecordException(LogMessageLevel level, string message, Exception ex)
        {
            var recordedExceptionMessage = new RecordedLogException(level, message, ex);

            events.Add(recordedExceptionMessage);
        }

        readonly MemoryStream stream;
        readonly List<RecordedLogMessage> events;
    }
}
