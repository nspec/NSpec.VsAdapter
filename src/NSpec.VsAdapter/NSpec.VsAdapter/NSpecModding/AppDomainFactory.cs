using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.NSpecModding
{
    public class AppDomainFactory : IAppDomainFactory
    {
        public AppDomain Create(string assemblyPath)
        {
            const string targetAppDomainName = "NSpec.VsAdapter.NSpecDomainRunner.Run";
            const Evidence useCurrentAppDomainEvidence = null;

            string configFilePath = assemblyPath + ".config";

            var domainInfo = new AppDomainSetup();

            domainInfo.ConfigurationFile = Path.GetFullPath(configFilePath);

            Assembly currentAssembly = Assembly.GetExecutingAssembly();

            domainInfo.ApplicationBase = Path.GetDirectoryName(currentAssembly.Location);

            var targetDomain = AppDomain.CreateDomain(targetAppDomainName, useCurrentAppDomainEvidence, domainInfo);

            var resolveHandler = new AssemblyResolveHandler(assemblyPath);

            targetDomain.AssemblyResolve += resolveHandler.Failed;

            return targetDomain;
        }

        class AssemblyResolveHandler
        {
            public AssemblyResolveHandler(string assemblyPath)
            {
                this.assemblyPath = assemblyPath;
            }

            public Assembly Failed(object sender, ResolveEventArgs eventArgs)
            {
                var name = eventArgs.Name;

                var argNameForResolve = eventArgs.Name.ToLower();

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

                var missing = Path.Combine(Path.GetDirectoryName(assemblyPath), name);

                if (File.Exists(missing))
                {
                    return Assembly.LoadFrom(missing);
                }

                return null;
            }

            string assemblyPath;
        }
    }
}
