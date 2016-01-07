using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter
{
    public class DebugInfoProvider : IDebugInfoProvider
    {
        public DebugInfoProvider(string binaryPath)
        {
            this.binaryPath = binaryPath;

            // TODO catch & log exceptions, then leave state incomplete

            this.session = new DiaSession(binaryPath);
        }

        // taken from https://github.com/nunit/nunit-vs-adapter/blob/master/src/NUnitTestAdapter/TestConverter.cs

        public DiaNavigationData GetNavigationData(string declaringClassName, string methodName)
        {
            var navData = session.GetNavigationData(declaringClassName, methodName);

            if (navData != null && navData.FileName != null)
            {
                return navData;
            }
            else
            {
                // TODO check if it's an async method, before leaving

                var noNavigationData = new DiaNavigationData(String.Empty, 0, 0);

                return noNavigationData;
            }
        }

        readonly string binaryPath;
        readonly DiaSession session;
    }
}
