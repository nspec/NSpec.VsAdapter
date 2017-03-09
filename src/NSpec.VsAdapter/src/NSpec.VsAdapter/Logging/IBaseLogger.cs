namespace NSpec.VsAdapter.Logging
{
    public interface IBaseLogger
    {
        void Trace(string message);

        void Debug(string message);

        void Info(string message);

        void Warn(string message);

        void Error(string message);
    }
}
