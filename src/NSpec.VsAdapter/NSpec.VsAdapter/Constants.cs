using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter
{
    public static class Constants
    {
        public const string DllExtension = ".dll";
        public const string ExeExtension = ".exe";

        // TODO change to something like executor://nspec-vsadapter-executor
        public const string ExecutorUriString = "executor://nspec-executor";

        public static Uri ExecutorUri = new Uri(ExecutorUriString);
    }
}
