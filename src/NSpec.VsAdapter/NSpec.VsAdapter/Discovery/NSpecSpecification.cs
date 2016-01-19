using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.VsAdapter.Discovery
{
    [Serializable]
    public class NSpecSpecification
    {
        // TODO rename in DiscoveredExample or SerializableSpec

        public string FullName { get; set; }

        public string SourceFilePath { get; set; }

        public int SourceLineNumber { get; set; }

        public string SourceAssembly { get; set; }

        public string[] Tags { get; set; }
    }
}
