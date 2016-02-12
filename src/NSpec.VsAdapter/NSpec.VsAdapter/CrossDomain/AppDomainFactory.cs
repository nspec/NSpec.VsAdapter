using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.CrossDomain
{
    public class AppDomainFactory : IAppDomainFactory
    {
        public ITargetAppDomain Create(string binaryPath)
        {
            const string targetDomainName = "NSpec.VsAdapter.AppDomainFactory";
            const Evidence useCurrentDomainEvidence = null;

            binaryPath = Path.GetFullPath(binaryPath);

            string configFilePath = binaryPath + ".config";

            Assembly currentAssembly = Assembly.GetExecutingAssembly();

            var targetDomainSetup = new AppDomainSetup()
            {
                ConfigurationFile = configFilePath,
                ApplicationBase = Path.GetDirectoryName(currentAssembly.Location)
            };
            // TODO verifiy if PrivateBinPath should be set as well

            var appDomain = AppDomain.CreateDomain(targetDomainName, useCurrentDomainEvidence, targetDomainSetup);

            var targetDomain = new TargetAppDomain(appDomain, binaryPath);

            return targetDomain;
        }
    }
}
