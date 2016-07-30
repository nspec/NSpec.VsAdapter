using System;

namespace NSpec.VsAdapter
{
    public static class Constants
    {
        public const string DllExtension = ".dll";
        public const string ExeExtension = ".exe";

        public const string ExecutorUriString = "executor://nspec-vsadapter-executor";

        public static Uri ExecutorUri = new Uri(ExecutorUriString);
    }
}
