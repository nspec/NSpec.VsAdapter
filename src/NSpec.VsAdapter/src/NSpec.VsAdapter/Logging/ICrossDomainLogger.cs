namespace NSpec.VsAdapter.Logging
{
    public interface ICrossDomainLogger : IBaseLogger
    {
        // exception info and message

        void Trace(ExceptionLogInfo exceptionInfo, string message);

        void Debug(ExceptionLogInfo exceptionInfo, string message);

        void Info(ExceptionLogInfo exceptionInfo, string message);

        void Warn(ExceptionLogInfo exceptionInfo, string message);

        void Error(ExceptionLogInfo exceptionInfo, string message);
    }
}
