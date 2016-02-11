using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.CrossDomain
{
    [Serializable]
    class AssemblyResolver
    {
        public AssemblyResolver(string binaryPath)
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
