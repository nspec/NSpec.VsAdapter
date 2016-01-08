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
        public ITargetAppDomain Create(string assemblyPath)
        {
            const string targetDomainName = "NSpec.VsAdapter.NSpecDomainRunner.Run";
            const Evidence useCurrentDomainEvidence = null;

            string configFilePath = assemblyPath + ".config";

            var targetDomainSetup = new AppDomainSetup();

            targetDomainSetup.ConfigurationFile = Path.GetFullPath(configFilePath);

            Assembly currentAssembly = Assembly.GetExecutingAssembly();

            targetDomainSetup.ApplicationBase = Path.GetDirectoryName(currentAssembly.Location);

            var appDomain = AppDomain.CreateDomain(targetDomainName, useCurrentDomainEvidence, targetDomainSetup);

            var resolveHandler = new AssemblyResolveHandler(assemblyPath);

            appDomain.AssemblyResolve += resolveHandler.Failed;

            var targetDomain = new TargetAppDomain(appDomain);

            return targetDomain;
        }

        [Serializable]
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
