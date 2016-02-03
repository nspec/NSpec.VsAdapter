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
            const string targetDomainName = "NSpec.VsAdapter.AppDomainFactory.Create";
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

            var resolveHandler = new AssemblyResolveHandler(binaryPath);

            appDomain.AssemblyResolve += resolveHandler.Failed;

            var targetDomain = new TargetAppDomain(appDomain, resolveHandler.Failed);

            return targetDomain;
        }

        [Serializable]
        class AssemblyResolveHandler
        {
            public AssemblyResolveHandler(string binaryPath)
            {
                this.binaryPath = binaryPath;
            }

            public Assembly Failed(object sender, ResolveEventArgs eventArgs)
            {
                var name = eventArgs.Name;

                var argNameForResolve = name.ToLower();

                if (argNameForResolve.Contains(","))
                {
                    name = argNameForResolve.Split(',').First() + ".dll";
                }
                else if (!argNameForResolve.EndsWith(".dll") && !argNameForResolve.Contains(".resource"))
                {
                    name += ".dll";
                }
                else if (argNameForResolve.Contains(".resource"))
                {
                    name = argNameForResolve.Substring(0, argNameForResolve.IndexOf(".resource")) + ".xml";
                }

                var missing = Path.Combine(Path.GetDirectoryName(binaryPath), name);

                if (File.Exists(missing))
                {
                    return Assembly.LoadFrom(missing);
                }

                return null;
            }

            string binaryPath;
        }
    }
}
