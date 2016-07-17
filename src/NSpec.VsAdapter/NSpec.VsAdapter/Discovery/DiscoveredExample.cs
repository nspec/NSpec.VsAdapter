using System;

namespace NSpec.VsAdapter.Discovery
{
    [Serializable]
    public class DiscoveredExample
    {
        public string FullName { get; set; }

        public string SourceFilePath { get; set; }

        public int SourceLineNumber { get; set; }

        public string SourceAssembly { get; set; }

        public string[] Tags { get; set; }
    }
}
