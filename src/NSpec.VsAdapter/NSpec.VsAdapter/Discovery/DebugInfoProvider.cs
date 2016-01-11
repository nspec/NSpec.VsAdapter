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

            // TODO catch & log exceptions, then leave state incomplete

            this.session = new DiaSession(binaryPath);
        }

        // taken from https://github.com/nunit/nunit-vs-adapter/blob/master/src/NUnitTestAdapter/TestConverter.cs

        public DiaNavigationData GetNavigationData(string declaringClassName, string methodName)
        {
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

                var noNavigationData = new DiaNavigationData(String.Empty, 0, 0);

                return noNavigationData;
            }
        }

        readonly string binaryPath;
        readonly ISerializableLogger logger;
        readonly DiaSession session;
    }
}
