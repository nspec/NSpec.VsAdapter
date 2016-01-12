using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Discovery
{
    public class DebugInfoProvider : IDebugInfoProvider
    {
        public DebugInfoProvider(string binaryPath, ISerializableLogger logger)
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

                logger.Warn(message);

                session = noSession;
            }
        }

        // taken from https://github.com/nunit/nunit-vs-adapter/blob/master/src/NUnitTestAdapter/TestConverter.cs

        public DiaNavigationData GetNavigationData(string declaringClassName, string methodName)
        {
            var noNavigationData = new DiaNavigationData(String.Empty, 0, 0);

            if (session == noSession)
            {
                return noNavigationData;
            }

            var navData = session.GetNavigationData(declaringClassName, methodName);

            if (navData != null && navData.FileName != null)
            {
                string message = String.Format("Debug info found for method '{0}'.'{1}' in binary '{2}'",
                    declaringClassName, methodName, binaryPath);

                logger.Debug(message);

                return navData;
            }
            else
            {
                string message = String.Format("Cannot get debug info for method '{0}'.'{1}' in binary '{2}'", 
                    declaringClassName, methodName, binaryPath);
                
                logger.Warn(message);

                // TODO check if it's an async method, before leaving

                return noNavigationData;
            }
        }

        readonly string binaryPath;
        readonly ISerializableLogger logger;
        readonly DiaSession session;

        readonly DiaSession noSession = null;
    }
}
