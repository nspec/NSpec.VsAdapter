using NSpec.VsAdapter.Logging;
using System;

namespace NSpec.VsAdapter.Execution
{
    public interface IProxyableTestExecutor : IDisposable
    {
        int ExecuteAll(string binaryPath,
            IProgressRecorder progressRecorder, ICrossDomainLogger logger);

        int ExecuteSelection(string binaryPath, string[] exampleFullNames,
            IProgressRecorder progressRecorder, ICrossDomainLogger logger);
    }
}
