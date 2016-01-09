using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSpec.VsAdapter
{
    [ProtoContract]
    [ProtoInclude(10, typeof(RecordedLogException))]
    public class RecordedLogMessage
    {
        public RecordedLogMessage() { }  // needed for deserialization

        public RecordedLogMessage(LogMessageLevel logLevel, string message)
        {
            LogLevel = logLevel;
            Message = message;
        }

        [ProtoMember(1)]
        public LogMessageLevel LogLevel { get; set; }

        [ProtoMember(2)]
        public string Message { get; set; }
    }

    [ProtoContract]
    public class RecordedLogException : RecordedLogMessage
    {
        public RecordedLogException() { }  // needed for deserialization

        public RecordedLogException(LogMessageLevel logLevel, string message, Exception ex)
            : base(logLevel, message)
        {
            ExceptionInfo = ex;
        }

        [ProtoMember(3)]
        public ExceptionLogInfo ExceptionInfo { get; set; }
    }

    [ProtoContract]
    public class ExceptionLogInfo
    {
        public ExceptionLogInfo() { }  // needed for deserialization

        public ExceptionLogInfo(Exception ex)
        {
            Type = ex.GetType().ToString();
            Content = ex.ToString();
        }

        [ProtoMember(1)]
        public string Type { get; set; }

        [ProtoMember(2)]
        public string Content { get; set; }

        static public implicit operator ExceptionLogInfo(Exception ex)
        {
            return new ExceptionLogInfo(ex);
        }
    }

    public enum LogMessageLevel
    {
        Unassigned,

        Debug,
        Info,
        Warn,
        Error,
    }
}
