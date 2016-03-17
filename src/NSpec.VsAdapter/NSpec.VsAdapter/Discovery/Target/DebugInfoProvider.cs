using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NSpec.VsAdapter.Logging;
using System;

namespace NSpec.VsAdapter.Discovery.Target
{
    public class DebugInfoProvider : IDebugInfoProvider
    {
        public DebugInfoProvider(string binaryPath, ICrossDomainLogger logger)
        {
            this.binaryPath = binaryPath;
            this.logger = logger;

            try
            {
                session = new DiaSession(binaryPath);
            }
            catch (Exception ex)
            {
                string message = String.Format("Cannot setup debug info for binary '{0}'", binaryPath);

                logger.Debug(new ExceptionLogInfo(ex), message);

                session = noSession;
            }

            try
            {
                asyncMethodHelper = new AsyncMethodHelper(binaryPath);
            }
            catch (Exception ex)
            {
                string message = String.Format("Cannot setup async debug info for binary '{0}'", binaryPath);

                logger.Debug(new ExceptionLogInfo(ex), message);

                asyncMethodHelper = noAsyncHelper;
            }
        }

        // taken from https://github.com/nunit/nunit-vs-adapter/blob/master/src/NUnitTestAdapter/TestConverter.cs

        public DiaNavigationData GetNavigationData(string declaringClassName, string methodName)
        {
            if (session == noSession)
            {
                return NoNavigationData();
            }

            var navData = session.GetNavigationData(declaringClassName, methodName);

            if (navData == null || navData.FileName == null)
            {
                if (asyncMethodHelper != noAsyncHelper)
                {
                    string stateMachineClassName = asyncMethodHelper.GetClassNameForAsyncMethod(declaringClassName, methodName);

                    if (stateMachineClassName != null)
                    {
                        navData = session.GetNavigationData(stateMachineClassName, "MoveNext");
                    }
                }
            }

            if (navData != null && navData.FileName != null)
            {
                string message = String.Format("Debug info found for method '{0}'.'{1}' in binary '{2}'",
                    declaringClassName, methodName, binaryPath);

                logger.Trace(message);

                return navData;
            }
            else
            {
                string message = String.Format("Cannot get debug info for method '{0}'.'{1}' in binary '{2}'",
                    declaringClassName, methodName, binaryPath);

                logger.Debug(message);

                return NoNavigationData();
            }
        }

        DiaNavigationData NoNavigationData()
        {
            return new DiaNavigationData(String.Empty, 0, 0);
        }

        readonly string binaryPath;
        readonly ICrossDomainLogger logger;
        readonly DiaSession session;
        readonly AsyncMethodHelper asyncMethodHelper;

        readonly DiaSession noSession = null;
        readonly AsyncMethodHelper noAsyncHelper = null;
    }
}
